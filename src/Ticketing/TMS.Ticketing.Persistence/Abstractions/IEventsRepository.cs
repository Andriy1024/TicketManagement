using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Persistence.Abstractions;

public interface IEventsRepository : IRepository<EventEntity, Guid> { }