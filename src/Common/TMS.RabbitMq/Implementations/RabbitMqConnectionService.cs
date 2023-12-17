using System.Net.Sockets;

using Polly;
using Polly.CircuitBreaker;

using RabbitMQ.Client.Exceptions;

using TMS.RabbitMq.Configuration;

namespace TMS.RabbitMq.Implementations;

internal sealed class RabbitMqConnectionService : IRabbitMqConnectionService
{
    #region Private members

    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMqConnectionService> _logger;
    private IConnection? _connection;
    private bool _isDisposed;
    private readonly ReaderWriterLockSlim _syncRoot = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    private readonly CircuitBreakerPolicy _circuitBreaker;
    private readonly RabbitMqConfiguration _options;
    
    #endregion

    public RabbitMqConnectionService(IOptions<RabbitMqConfiguration > messageBusOptions, ILogger<RabbitMqConnectionService> logger)
    {
        _logger = logger;
        _options = RabbitMqConfiguration .Validate(messageBusOptions.Value);
        _connectionFactory = new ConnectionFactory
        {
            UserName = _options.Connection.UserName,
            Password = _options.Connection.Password,
            VirtualHost = _options.Connection.VirtualHost,
            HostName = _options.Connection.HostName,
            Port = Convert.ToInt32(_options.Connection.Port),
            DispatchConsumersAsync = true,
        };

        _circuitBreaker = Policy
            .Handle<Exception>()
            .CircuitBreaker(
                _options.Resilience.ExceptionsAllowedBeforeBreaking, 
                TimeSpan.FromSeconds(_options.Resilience.DurationOfBreak));
    }

    #region IRabbitMqConnectionService members

    public bool IsConnected
    {
        get
        {
            using (new ReadLock(_syncRoot))
                return _connection != null && _connection.IsOpen && !_isDisposed;
        }
    }

    public IModel CreateChannel()
    {
        if (!IsConnected)
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");

        using (new ReadLock(_syncRoot))
            return _connection!.CreateModel();
    }

    public bool TryConnect()
    {
        if (IsConnected) return true;

        _logger.LogInformation("RabbitMQ Client is trying to connect");

        using (new WriteLock(_syncRoot)) 
        {
            var retryPolicy = Policy
                .Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_options.Resilience.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                });

            _circuitBreaker.Wrap(retryPolicy).Execute(() => _connection = _connectionFactory.CreateConnection());

            if (_connection != null && _connection.IsOpen)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;
                
                _logger.LogInformation($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");
                
                return true;
            }
            else
            {
                _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                
                return false;
            }
        }
    }

    #endregion

    #region IDisposable members

    public void Dispose()
    {
        using (new WriteLock(_syncRoot)) 
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            try
            {
                _connection?.Dispose();
                _connection = null;
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }

    #endregion

    #region Private methods

    private bool IsDisposed()
    {
        using (new ReadLock(_syncRoot))
            return _isDisposed;
    }

    #endregion

    #region Event handlers

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        if (IsDisposed()) return;

        _logger.LogError($"A RabbitMQ connection is blocked. Reason: {e.Reason} Trying to re-connect...");

        TryConnect();
    }

    private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
        if (IsDisposed()) return;

        if (e.Exception != null)
        {
            _logger.LogError(e.Exception, $"A RabbitMQ connection throw exception. {e.Exception.Message}. Trying to re-connect...");

            TryConnect();
        }
    }

    private void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
    {
        if (IsDisposed()) return;

        if (reason != null)
        {
            _logger.LogError($"A RabbitMQ connection is on shutdown. {reason.ReplyText}. Trying to re-connect...");

            TryConnect();
        }
    }

    #endregion
}
