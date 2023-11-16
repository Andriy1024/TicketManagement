using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.Repositories;

public interface IEventsRepository : IRepository<EventEntity, Guid> { }