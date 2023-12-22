using TMS.Common.Errors;
using TMS.Ticketing.Infrastructure.ChangeTracker;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;
using TMS.Ticketing.Persistence.Sessions;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class EventsRepository : ChangeTrackableRepository<EventEntity, Guid>, IEventsRepository
{
    protected override string CollectionName => Collections.Events;

    public EventsRepository(IMongoDatabase database, MongoTransactionScope transactionScope, IEntityChangeTracker domainEvents)
         : base(database, transactionScope, domainEvents) {}

    public async Task<EventEntity> GetRequiredAsync(Guid eventId)
        => await GetAsync(eventId)
            ?? throw ApiError.NotFound($"Event not found: {eventId}").ToException();
}