using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Persistence.Abstractions;

public interface ICartsRepository : IRepository<CartEntity, Guid> { }