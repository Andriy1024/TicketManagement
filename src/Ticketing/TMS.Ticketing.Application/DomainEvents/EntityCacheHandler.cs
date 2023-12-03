using TMS.Ticketing.Application.Helpers;
using TMS.Ticketing.Domain.DomainEvents;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Application.DomainEvents;

internal sealed class EntityCacheHandler :
    INotificationHandler<EntityUpdated<EventEntity>>,
    INotificationHandler<EntityDeleted<EventEntity>>,
    INotificationHandler<EntityUpdated<VenueEntity>>,
    INotificationHandler<EntityDeleted<VenueEntity>>,
    INotificationHandler<EntityCreated<VenueBookingEntity>>
{
    private readonly ICacheClient _cache;

    public EntityCacheHandler(ICacheClient cache)
        => _cache = cache;

    public Task Handle(EntityUpdated<EventEntity> notification, CancellationToken cancellationToken)
        => ClearCache(notification.Entity, _cache);

    public Task Handle(EntityDeleted<EventEntity> notification, CancellationToken cancellationToken)
        => ClearCache(notification.Entity, _cache);

    public Task Handle(EntityUpdated<VenueEntity> notification, CancellationToken cancellationToken)
        => ClearCache(notification.Entity, _cache);

    public Task Handle(EntityDeleted<VenueEntity> notification, CancellationToken cancellationToken)
        => ClearCache(notification.Entity, _cache);

    public Task Handle(EntityCreated<VenueBookingEntity> notification, CancellationToken cancellationToken)
        => _cache.DeleteAsync(CacheKeys.GetVenueBookingKey(notification.Entity.Id));

    private static Task ClearCache(EventEntity @event, ICacheClient cache)
        => cache.DeleteAsync(CacheKeys.GetEventKey(@event.Id));

    private static Task ClearCache(VenueEntity venue, ICacheClient cache)
        => cache.DeleteAsync(CacheKeys.GetVenueKey(venue.Id));   
}