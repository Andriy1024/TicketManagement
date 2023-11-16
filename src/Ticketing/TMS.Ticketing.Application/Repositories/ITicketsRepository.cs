using TMS.Ticketing.Domain.Tickets;

namespace TMS.Ticketing.Application.Repositories;

public interface ITicketsRepository : IRepository<TicketEntity, Guid> { }
