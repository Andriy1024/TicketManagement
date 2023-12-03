using TMS.Common.Interfaces;
using TMS.Ticketing.Domain;

namespace TMS.Ticketing.Infrastructure.ChangeTracker;

public interface IEntityChangeTracker
{
    IEnumerable<IDomainEvent> ExtractEvents();

    void Add(IEntity entity);
}