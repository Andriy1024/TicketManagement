using TMS.Common.Errors;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;
using TMS.Ticketing.Persistence.Sessions;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class CartsRepository : MongoRepository<CartEntity, Guid>, ICartsRepository
{
    protected override string CollectionName => Collections.Carts;

    public CartsRepository(IMongoDatabase database, MongoTransactionScope transactionScope) 
        : base(database, transactionScope) {}

    public async Task<CartEntity> GetRequiredAsync(Guid id)
        => await GetAsync(id)
            ?? throw ApiError.NotFound($"Cart not found: {id}").ToException();
}