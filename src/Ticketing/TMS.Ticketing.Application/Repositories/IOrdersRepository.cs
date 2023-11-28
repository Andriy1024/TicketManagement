using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Application.Repositories;

public interface IOrdersRepository : IRepository<OrderEntity, Guid> { }
