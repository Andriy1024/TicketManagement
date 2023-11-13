using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Persistence.Abstractions;

public interface IOrdersRepository : IRepository<OrderEntity, Guid> { }
