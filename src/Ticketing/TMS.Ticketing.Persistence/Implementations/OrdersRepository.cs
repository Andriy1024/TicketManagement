using MongoDB.Driver;

using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class OrdersRepository : MongoRepository<OrderEntity, Guid>, IOrdersRepository
{
    protected override string CollectionName => Collections.Orders;

    public OrdersRepository(IMongoDatabase database) : base(database)
    {
    }
}
