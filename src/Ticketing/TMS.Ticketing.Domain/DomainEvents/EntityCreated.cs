using TMS.Common.Interfaces;

namespace TMS.Ticketing.Domain.DomainEvents;

public record EntityCreated<TEntity>(TEntity Entity) : IDomainEvent;