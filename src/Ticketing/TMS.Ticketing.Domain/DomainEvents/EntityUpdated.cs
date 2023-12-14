using TMS.Common.Interfaces;

namespace TMS.Ticketing.Domain.DomainEvents;

public record EntityUpdated<TEntity>(TEntity Entity) : IDomainEvent;