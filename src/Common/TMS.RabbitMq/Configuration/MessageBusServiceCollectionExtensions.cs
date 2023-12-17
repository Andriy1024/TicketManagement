using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using TMS.RabbitMq.Implementations;
using TMS.RabbitMq.Publisher;
using TMS.RabbitMq.Subscriber;

namespace TMS.RabbitMq.Configuration;

public static class MessageBusServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqMessageBus(this IServiceCollection services, IConfiguration configuration)
        => services.AddRabbitMqMessageBus<MessageDispatcher>(configuration);

    public static IServiceCollection AddRabbitMqMessageBus<TMessageDispatcher>(
        this IServiceCollection services, IConfiguration configuration)
        where TMessageDispatcher : class, IMessageDispatcher
    {
        return services
            .Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"))
            .AddSingleton<IMessageBroker, MessageBroker>()
            .AddSingleton<IRabbitMqSubscriber, RabbitMqSubscriber>()
            .AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>()
            .AddSingleton<IMessageSerializer, DefaultMessageSerializer>()
            .AddSingleton<IMessageDispatcher, TMessageDispatcher>()
            .AddSingleton<IRabbitMqConnectionService, RabbitMqConnectionService>();
    }
}