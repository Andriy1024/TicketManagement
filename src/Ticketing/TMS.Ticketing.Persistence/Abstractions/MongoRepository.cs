using System.Linq.Expressions;

using TMS.Ticketing.Persistence.Sessions;

namespace TMS.Ticketing.Persistence.Abstractions;

internal abstract class MongoRepository<TEntity, TIdentifiable> : IRepository<TEntity, TIdentifiable>
    where TEntity : IEntity<TIdentifiable>
    where TIdentifiable : notnull
{
    protected abstract string CollectionName { get; }

    protected IMongoCollection<TEntity> Collection { get; }

    protected MongoTransactionScope TransactionScope { get; }

    public MongoRepository(IMongoDatabase database, MongoTransactionScope transactionScope)
    {
        Collection = database.GetCollection<TEntity>(CollectionName);
        TransactionScope = transactionScope;
    }

    public Task<TEntity?> GetAsync(TIdentifiable id, CancellationToken cancellationToken = default)
        => GetAsync(e => e.Id.Equals(id), cancellationToken);

    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        => Collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.AsQueryable().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await Collection.Find(predicate).ToListAsync(cancellationToken);

    public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => TransactionScope.HasTransaction
            ? Collection.InsertOneAsync(TransactionScope.GetTransaction(), entity, cancellationToken: cancellationToken)
            : Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        => UpdateAsync(entity, e => e.Id.Equals(entity.Id), cancellationToken: cancellationToken);

    public virtual Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => TransactionScope.HasTransaction
            ? Collection.ReplaceOneAsync(TransactionScope.GetTransaction(), predicate, entity, cancellationToken: cancellationToken)
            : Collection.ReplaceOneAsync(predicate, entity, cancellationToken: cancellationToken);
    
    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        => DeleteAsync(e => e.Id.Equals(entity.Id), cancellationToken);

    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => TransactionScope.HasTransaction
            ? Collection.DeleteOneAsync(TransactionScope.GetTransaction(), predicate, cancellationToken: cancellationToken)
            : Collection.DeleteOneAsync(predicate, cancellationToken: cancellationToken);
    
    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => Collection.Find(predicate).AnyAsync(cancellationToken);
}