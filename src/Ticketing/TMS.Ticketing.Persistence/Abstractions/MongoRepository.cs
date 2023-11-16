﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;

using TMS.Ticketing.Domain;

using System.Linq.Expressions;

namespace TMS.Ticketing.Persistence.Abstractions;

internal abstract class MongoRepository<TEntity, TIdentifiable> : IRepository<TEntity, TIdentifiable>
    where TEntity : IEntity<TIdentifiable>
    where TIdentifiable : notnull
{
    protected abstract string CollectionName { get; }

    protected IMongoCollection<TEntity> Collection { get; }

    // TEMP
    public IQueryable<TEntity> Qearyable => Collection.AsQueryable();

    public MongoRepository(IMongoDatabase database)
    {
        Collection = database.GetCollection<TEntity>(CollectionName);
    }

    public Task<TEntity?> GetAsync(TIdentifiable id, CancellationToken cancellationToken = default)
        => GetAsync(e => e.Id.Equals(id), cancellationToken);

    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => Collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.AsQueryable().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await Collection.Find(predicate).ToListAsync(cancellationToken);

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        => UpdateAsync(entity, e => e.Id.Equals(entity.Id), cancellationToken: cancellationToken);

    public Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => Collection.ReplaceOneAsync(predicate, entity, cancellationToken: cancellationToken);

    public Task DeleteAsync(TIdentifiable id, CancellationToken cancellationToken = default)
        => DeleteAsync(e => e.Id.Equals(id), cancellationToken);

    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => Collection.DeleteOneAsync(predicate, cancellationToken: cancellationToken);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => Collection.Find(predicate).AnyAsync(cancellationToken);
}