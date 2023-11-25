using TMS.Ticketing.Domain.Tickets;

namespace TMS.Ticketing.Persistence.Abstractions;

public interface ITicketsRepository : IRepository<TicketEntity, Guid> { }
