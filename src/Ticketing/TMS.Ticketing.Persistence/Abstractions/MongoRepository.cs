using MongoDB.Driver;
using System.Linq.Expressions;
using System.Transactions;
using TMS.Ticketing.Domain;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Persistence.Exceptions;
using TMS.Ticketing.Persistence.Sessions;
using ZstdSharp;

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
    {
        return TransactionScope.HasTransaction
            ? Collection.InsertOneAsync(TransactionScope.GetTransaction(), entity, cancellationToken: cancellationToken)
            : Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var versionInfo = entity.IncreaseVersion();

        FilterDefinition<TEntity> filter = 
             Builders<TEntity>.Filter.Eq(r => r.Id, entity.Id)
           & Builders<TEntity>.Filter.Eq(r => r.Version, versionInfo.Old);

        var result = TransactionScope.HasTransaction
            ? await Collection.ReplaceOneAsync(TransactionScope.GetTransaction(), filter, entity, cancellationToken: cancellationToken)
            : await Collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);

        if (!result.IsAcknowledged)
        {
            throw MongoDbException.OperationIsAcknowledged();
        }

        if (result.ModifiedCount != 1)
        {
            throw new MongoDbException($"This operation conflicted with another operation. MongoDB modified count expected to be one, actual: {result.ModifiedCount}. Please retry your operation.");
        }
    }
    
    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var versionInfo = entity.IncreaseVersion();

        FilterDefinition<TEntity> filter =
            Builders<TEntity>.Filter.Eq(r => r.Id, entity.Id)
          & Builders<TEntity>.Filter.Eq(r => r.Version, versionInfo.Old);

        var result = TransactionScope.HasTransaction
            ? await Collection.DeleteOneAsync(TransactionScope.GetTransaction(), filter, cancellationToken: cancellationToken)
            : await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);

        if (!result.IsAcknowledged)
        {
            throw MongoDbException.OperationIsAcknowledged();
        }

        if (result.DeletedCount != 1)
        {
            throw new MongoDbException($"This operation conflicted with another operation. MongoDB delete count expected to be one, actual: {result.DeletedCount}. Please retry your operation.");
        }
    }
    
    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => Collection.Find(predicate).AnyAsync(cancellationToken);
}