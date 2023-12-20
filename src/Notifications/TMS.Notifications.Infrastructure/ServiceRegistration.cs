using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TMS.Common.Interfaces;
using TMS.Notifications.Application.IntegrationEvents;
using TMS.Notifications.Infrastructure.BackgroundServices;
using TMS.Notifications.Infrastructure.MessageBrocker;

using TMS.RabbitMq.Configuration;

namespace TMS.Notifications.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddRabbitMqMessageBus(configuration)
            .AddTransient<IStartupTask, RabbitMqStartupTask>()
            .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<OrderStatusUpdatedNotificationHandler>())
            .AddHostedService<NotificationsBackgroundService>();
    }
}