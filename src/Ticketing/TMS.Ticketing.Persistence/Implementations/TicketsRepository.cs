using TMS.Ticketing.Domain.Tickets;
using TMS.Ticketing.Infrastructure.ChangeTracker;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class TicketsRepository : ChangeTrackableRepository<TicketEntity, Guid>, ITicketsRepository
{
    protected override string CollectionName => Collections.Tickets;

    public TicketsRepository(IMongoDatabase database, MongoTransactionScope transactionScope, IEntityChangeTracker domainEvents) 
        : base(database, transactionScope, domainEvents) {}
}
