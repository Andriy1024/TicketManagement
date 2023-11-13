using MongoDB.Driver;

using System.Linq.Expressions;

using TMS.Ticketing.Domain;

namespace TMS.Ticketing.Persistence.Abstractions;

public interface IMongoRepository<TEntity, TIdentifiable>
    where TEntity : IDocumentEntry<TIdentifiable>
{
    public IMongoCollection<TEntity> Collection { get; }
    Task<TEntity> GetAsync(TIdentifiable id, CancellationToken cancellationToken = default);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task DeleteAsync(TIdentifiable id, CancellationToken cancellationToken = default);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}