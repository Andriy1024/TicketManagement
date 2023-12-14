using TMS.Common.Interfaces;

namespace TMS.Ticketing.Domain.DomainEvents;

public record EntityDeleted<TEntity>(TEntity Entity) : IDomainEvent;