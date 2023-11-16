using MongoDB.Driver;

using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class CartsRepository : MongoRepository<CartEntity, Guid>, ICartsRepository
{
    protected override string CollectionName => Collections.Carts;

    public CartsRepository(IMongoDatabase database) : base(database)
    {
    }
}
