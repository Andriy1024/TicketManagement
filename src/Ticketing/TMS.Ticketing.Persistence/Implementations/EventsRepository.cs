using MongoDB.Driver;
using TMS.Common.Errors;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class EventsRepository : MongoRepository<EventEntity, Guid>, IEventsRepository
{
    protected override string CollectionName => Collections.Events;

    public EventsRepository(IMongoDatabase database) : base(database)
    {
    }

    public async Task<EventEntity> GetRequiredAsync(Guid eventId)
    {
        return await GetAsync(eventId)
            ?? throw ApiError.NotFound($"Event not found: {eventId}").ToException();
    }
}