using TMS.Common.Errors;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Infrastructure.ChangeTracker;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.Persistence.Helpers;

namespace TMS.Ticketing.Persistence.Implementations;

internal sealed class CartsRepository : ChangeTrackableRepository<CartEntity, Guid>, ICartsRepository
{
    protected override string CollectionName => Collections.Carts;

    public CartsRepository(IMongoDatabase database, MongoTransactionScope transactionScope, IEntityChangeTracker domainEvents) 
        : base(database, transactionScope, domainEvents) {}

    public async Task<CartEntity> GetRequiredAsync(Guid id)
        => await GetAsync(id)
            ?? throw ApiError.NotFound($"Cart not found: {id}").ToException();
}