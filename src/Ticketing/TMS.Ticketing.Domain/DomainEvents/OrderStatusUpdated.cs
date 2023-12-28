using TMS.Common.Interfaces;
using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Domain.DomainEvents;

public sealed record OrderStatusUpdated(OrderEntity Order) : IDomainEvent
{
}