using MongoDB.Driver;

using TMS.Ticketing.Domain.Tickets;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Database;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class TicketsRepository : MongoRepository<TicketEntity, Guid>, ITicketsRepository
{
    protected override string CollectionName => Collections.Tickets;

    public TicketsRepository(IMongoDatabase database) : base(database)
    {
    }
}
