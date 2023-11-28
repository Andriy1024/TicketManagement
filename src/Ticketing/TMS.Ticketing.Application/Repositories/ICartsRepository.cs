using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Application.Repositories;

public interface ICartsRepository : IRepository<CartEntity, Guid> 
{
    Task<CartEntity> GetRequiredAsync(Guid id);
}