using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson.Serialization;

using TMS.Common.IntegrationEvents.Notifications;

using TMS.Notifications.Application.Interfaces;
using TMS.Notifications.Domain.Models;
using TMS.Notifications.Persistence.Repositories;

using TMS.MongoDB;

namespace TMS.Notifications.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        BsonClassMap.RegisterClassMap<NotificationPayload>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIsRootClass(true);
            classMap.SetDiscriminator("NotificationPayload");
        });

        BsonClassMap.RegisterClassMap<OrderStatusUpdatedNotification>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetDiscriminator("OrderStatusUpdatedNotification");
        });

        BsonClassMap.RegisterClassMap<NotificationEntity>(classMap => {
            classMap.AutoMap();
            classMap.MapIdMember(x => x.Id);
        });

        return services
            .AddMongoDBServices(configuration)
            .AddScoped<INotificationsRepository, NotificationsRepository>();
    }
}