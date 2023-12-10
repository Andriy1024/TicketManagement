using System.Linq.Expressions;

using TMS.Ticketing.Infrastructure.ChangeTracker;
using TMS.Ticketing.Persistence.Sessions;

namespace TMS.Ticketing.Persistence.Abstractions;

internal abstract class ChangeTrackableRepository<TEntity, TIdentifiable> : MongoRepository<TEntity, TIdentifiable>
    where TEntity : IEntity<TIdentifiable>
    where TIdentifiable : notnull
{
    private readonly IEntityChangeTracker _changeTracker;

    protected ChangeTrackableRepository(IMongoDatabase database, MongoTransactionScope transactionScope, IEntityChangeTracker domainEvents)
        : base(database, transactionScope)
    {
        _changeTracker = domainEvents;
    }

    public override Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _changeTracker.Add(entity);

        return base.AddAsync(entity, cancellationToken);
    }

    public override Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _changeTracker.Add(entity);

        return base.DeleteAsync(entity, cancellationToken);
    }

    public override Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        _changeTracker.Add(entity);

        return base.UpdateAsync(entity, predicate, cancellationToken);
    }
}