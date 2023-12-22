using TMS.Common.Interfaces;
using TMS.Ticketing.Domain;

namespace TMS.Ticketing.Infrastructure.ChangeTracker;

internal sealed class EntityChangeTracker : IEntityChangeTracker
{
    private readonly List<IEntity> _entities = new();

    public void Add(IEntity entity)
    {
        _entities.Add(entity);
    }

    public IEnumerable<IDomainEvent> ExtractEvents()
    {
        var result = _entities
            .Where(x => x.DomainEvents.Count != 0)
            .SelectMany(x => x.ExtractDomainEvents())
            .ToList();

        _entities.Clear();

        return result;
    }
}