using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Infrastructure.ChangeTracker;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class OrdersRepository : ChangeTrackableRepository<OrderEntity, Guid>, IOrdersRepository
{
    protected override string CollectionName => Collections.Orders;

    public OrdersRepository(IMongoDatabase database, MongoTransactionScope transactionScope, IEntityChangeTracker domainEvents) 
        : base(database, transactionScope, domainEvents) {}
}
