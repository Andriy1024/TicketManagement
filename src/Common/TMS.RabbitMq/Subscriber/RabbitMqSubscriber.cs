﻿using TMS.RabbitMq.Configuration;

using TMS.RabbitMq.Pipeline;

namespace TMS.RabbitMq.Subscriber;

public sealed class RabbitMqSubscriber : IRabbitMqSubscriber
{
    #region Private members

    private readonly ReaderWriterLockSlim _syncRootForSubscribers = new();
    private readonly Dictionary<SubscriptionKey, SubscriberContext> _subscribers = new();

    private readonly IRabbitMqConnectionService _connectionService;
    private readonly ILogger<RabbitMqSubscriber> _logger;

    private readonly IMessageSerializer _serializer;
    private readonly IMessageDispatcher _dispatcher;

    private readonly RabbitMqConfiguration _options;

    #endregion

    #region Constructors

    public RabbitMqSubscriber(IRabbitMqConnectionService connectionService,
        IMessageDispatcher dispatcher,
        IMessageSerializer serializer,
        ILogger<RabbitMqSubscriber> logger,
        IOptions<RabbitMqConfiguration> options)
    {
        _connectionService = connectionService;
        _dispatcher = dispatcher;
        _serializer = serializer;
        _logger = logger;
        _options = RabbitMqConfiguration.Validate(options.Value);
    }

    #endregion

    #region IRabbitMqSubscriber members

    public void Subscribe<T>(Action<SubscribeProperties> action)
        where T : IIntegrationEvent
    {
        var properties = new SubscribeProperties();

        action(properties);

        Subscribe<T>(properties);
    }

    public void Subscribe<T>(SubscribeProperties properties)
        where T : IIntegrationEvent
    {
        if (string.IsNullOrEmpty(properties.Queue.RoutingKey))
        {
            properties.Queue.RoutingKey = GetRoutingKey<T>();
        }

        if (string.IsNullOrEmpty(properties.Queue.Name))
        {
            properties.Queue.Name = GetQueueName(properties.Exchange.Name, properties.Queue.RoutingKey);
        }

        var key = new SubscriptionKey(properties.Exchange.Name, properties.Queue.RoutingKey);

        using (new ReadLock(_syncRootForSubscribers))
        {
            if (_subscribers.ContainsKey(key))
            {
                _logger.LogError($"Integration event of type {typeof(T).Name} already subscribed for '{key}'.");

                return;
            }
        }

        var subscriptionInfo = CreateSubscriberChannel<T>(key, properties);

        using (new WriteLock(_syncRootForSubscribers))
        {
            if (_subscribers.ContainsKey(key))
            {
                subscriptionInfo.Channel.Close();

                return;
            }
            else
            {
                _subscribers.Add(key, subscriptionInfo);
            }
        }

        _logger.LogInformation($"New subscription: {subscriptionInfo}");
    }

    public void Unsubscribe<T>(SubscribeProperties eventBusProperties)
        where T : IIntegrationEvent
    {
        var exchangeName = eventBusProperties.Exchange.Name;

        var routingKey = eventBusProperties.Queue?.RoutingKey ?? GetRoutingKey<T>();

        var key = new SubscriptionKey(exchangeName, routingKey);

        SubscriberContext? subscriptionInfo = null;

        bool exists = false;

        using (new ReadLock(_syncRootForSubscribers))
        {
            exists = _subscribers.TryGetValue(key, out subscriptionInfo);
        }

        if (!exists)
        {
            _logger.LogError($"Subscriber: {key} was not found.");

            return;
        }

        if (subscriptionInfo!.Channel != null)
        {
            if (!_connectionService.IsConnected)
            {
                _connectionService.TryConnect();
            }

            if (!subscriptionInfo.Channel.IsClosed)
            {
                subscriptionInfo.Channel.QueueUnbind(queue: subscriptionInfo.Queue.Name, exchange: exchangeName, routingKey: routingKey);

                subscriptionInfo.Channel.Close();
            }
        }

        using (new WriteLock(_syncRootForSubscribers))
        {
            _subscribers.Remove(key);
        }
    }

    #endregion

    #region Private methods

    private static string GetRoutingKey<T>() => typeof(T).Name;

    private string GetQueueName(string exchange, string routingKey) => $"{_options.ConnectionName}/{exchange}.{routingKey}";

    private SubscriberContext CreateSubscriberChannel<T>(SubscriptionKey key, SubscribeProperties properties)
        where T : IIntegrationEvent
    {
        if (!_connectionService.IsConnected && !_connectionService.TryConnect())
        {
            throw new Exception("Can't connect to RabbitMq.");
        }

        var channel = _connectionService.CreateChannel();

        channel.ExchangeDeclare(exchange: properties.Exchange.Name, type: properties.Exchange.Type, durable: properties.Exchange.Durable, autoDelete: properties.Exchange.AutoDelete);

        var queueName = channel.QueueDeclare(queue: properties.Queue.Name, durable: properties.Queue.Durable, exclusive: properties.Queue.Exclusive, autoDelete: properties.Queue.AutoDelete).QueueName;

        channel.QueueBind(queue: queueName, exchange: properties.Exchange.Name, routingKey: properties.Queue.RoutingKey);

        channel.BasicQos(0, properties.Consumer.PrefetchCount, false);

        var pipeBuilder = new Pipe.Builder<SubscriberRequest>();

        if (!properties.Consumer.Autoack)
        {
            pipeBuilder.Add(new AcknowledgmentPipe(channel, properties.Consumer.RequeueFailedMessages));
        }

        if (properties.Consumer.RetryOnFailure)
        {
            pipeBuilder.Add(new RetryPipe(_logger));
        }

        Pipe.Handler<SubscriberRequest> handler = pipeBuilder.Build(lastPipe: (r) => _dispatcher.HandleAsync(r));

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (sender, args) =>
        {
            await OnMessageReceived<T>(sender, args, handler);
        };

        channel.BasicConsume(queue: queueName, autoAck: properties.Consumer.Autoack, consumer: consumer);

        channel.CallbackException += (sender, ea) =>
        {
            _logger.LogError(ea.Exception, ea.Exception.Message);

            channel.Dispose();

            CreateSubscriberChannel<T>(key, properties);
        };

        return new SubscriberContext(key, EventType: typeof(T), channel, properties.Queue, properties.Exchange, properties.Consumer);
    }

    private async Task OnMessageReceived<T>(object sender, BasicDeliverEventArgs ea, Pipe.Handler<SubscriberRequest> handlerPipeline)
        where T : IIntegrationEvent
    {
        var message = _serializer.Deserialize<T>(ea.Body.Span);

        try
        {
            await handlerPipeline(new SubscriberRequest(ea, message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    #endregion
}
