﻿using Polly;
using Polly.CircuitBreaker;

using TMS.RabbitMq.Configuration;

using RabbitMQ.Client.Exceptions;

using System.Net.Sockets;

namespace TMS.RabbitMq.Publisher;

public sealed class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly ReaderWriterLockSlim _syncRootForPublishers = new ReaderWriterLockSlim();
    
    private readonly Dictionary<SubscriptionKey, PublisherContext> _publishers = new Dictionary<SubscriptionKey, PublisherContext>();

    private readonly IRabbitMqConnectionService _connectionService;
    
    private readonly ILogger<RabbitMqPublisher> _logger;
    
    private readonly IMessageSerializer _serializer;
    
    private readonly RabbitMqConfiguration _options;
    
    private readonly CircuitBreakerPolicy _circuitBreaker;
 
    public RabbitMqPublisher(IRabbitMqConnectionService connectionService,
        ILogger<RabbitMqPublisher> logger,
        IMessageSerializer serializer,
        IOptions<RabbitMqConfiguration> options)
    {
        _connectionService = connectionService;
        _logger = logger;
        _serializer = serializer;
        _options = RabbitMqConfiguration.Validate(options.Value);
        _circuitBreaker = Policy
            .Handle<Exception>()
            .CircuitBreaker(
                _options.Resilience.ExceptionsAllowedBeforeBreaking,
                TimeSpan.FromSeconds(_options.Resilience.DurationOfBreak));
    }

    public void AddPublisher(PublishProperties eventBusProperties)
    {
        InitPublisher(eventBusProperties);
    }

    public bool RemovePublisher(PublishProperties eventBusProperties)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(eventBusProperties.RoutingKey);

        var key = new SubscriptionKey(eventBusProperties.Exchange.Name, eventBusProperties.RoutingKey);

        PublisherContext publisher = null;

        var result = false;

        using (new WriteLock(_syncRootForPublishers))
        {
            if (_publishers.TryGetValue(key, out publisher))
            {
                result = _publishers.Remove(key);
            }
            else 
            {
                return true;
            }
        }

        if (result)
        {
            publisher.Close();

            _logger.LogInformation($"Removed publisher: {publisher}");
        }

        return result;
    }

    public void Publish<T>(T integrationEvent, Action<PublishProperties> action)
        where T : IIntegrationEvent
    {
        var properties = new PublishProperties();
        
        action(properties);
        
        Publish(integrationEvent, properties);
    }

    public void Publish<T>(T integrationEvent, PublishProperties eventBusProperties)
        where T : IIntegrationEvent
    {
        if (string.IsNullOrEmpty(eventBusProperties.RoutingKey)) 
        {
            eventBusProperties.RoutingKey = integrationEvent.GetType().Name;
        }

        var body = _serializer.SerializeToBytes(integrationEvent, integrationEvent.GetType());

        var publisher = InitPublisher(eventBusProperties);

        var props = publisher.Channel.CreateBasicProperties();

        if (eventBusProperties.EnableRetryPolicy) 
        {
            var retryPolicy = Policy
                .Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_options.Resilience.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                });

            _circuitBreaker.Wrap(retryPolicy).Execute(() =>
            {
                publisher.Publish(properties: props, message: body);
            });
        }
        else 
        {
            publisher.Publish(properties: props, message: body);
        }
    }

    private PublisherContext InitPublisher(PublishProperties properties)
    {
        if (!_connectionService.IsConnected) 
        {
            _connectionService.TryConnect();
        }

        var exchange = properties.Exchange;

        var key = new SubscriptionKey(exchange.Name, properties.RoutingKey);

        using (new ReadLock(_syncRootForPublishers))
        {
            if (_publishers.TryGetValue(key, out var publisher)) 
            {
                return publisher;
            }
        }

        var newChannel = _connectionService.CreateChannel();
        
        newChannel.ExchangeDeclare(exchange: exchange.Name, type: exchange.Type, durable: exchange.Durable, autoDelete: exchange.AutoDelete);

        using (new WriteLock(_syncRootForPublishers))
        {
            if (_publishers.TryGetValue(key, out var existedPublisher))
            {
                if (!newChannel.IsClosed)
                {
                    newChannel.Close();
                }

                return existedPublisher;
            }
            else
            {
                var publisher = new PublisherContext(exchange, properties.RoutingKey, newChannel);
                
                _publishers.Add(key, publisher);
                
                _logger.LogInformation($"Added publisher: {publisher}");
                
                return publisher;
            }
        }
    }
}