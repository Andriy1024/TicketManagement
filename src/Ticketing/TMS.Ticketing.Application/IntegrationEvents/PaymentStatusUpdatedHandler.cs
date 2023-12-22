using TMS.Common.IntegrationEvents;

using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Tickets;

namespace TMS.Ticketing.Application.IntegrationEvents;

internal sealed class PaymentStatusUpdatedHandler : IRequestHandler<IntegrationEvent<PaymentStatusUpdated>, Unit>
{
    private readonly IEventsRepository _eventsRepo;
    private readonly IOrdersRepository _ordersRepo;
    private readonly ITicketsRepository _ticketsRepo;

    public PaymentStatusUpdatedHandler(IEventsRepository eventsRepo, IOrdersRepository ordersRepo, ITicketsRepository ticketsRepo)
    {
        _eventsRepo = eventsRepo;
        _ordersRepo = ordersRepo;
        _ticketsRepo = ticketsRepo;
    }

    public async Task<Unit> Handle(IntegrationEvent<PaymentStatusUpdated> request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;

        var orders = await _ordersRepo.FindAsync(x => x.Id == payload.PaymentId);

        foreach (var order in orders) 
        {
            order.UpdateStatus(request.Payload.Status);

            var @event = await _eventsRepo.GetRequiredAsync(order.EventId);

            if (order.Status == OrderStatus.Completed)
            {
                @event.SellSeat(order.OrderItems.Select(x => x.SeatId));

                await CreateTicketsAsync(order);
            }
            else
            {
                @event.ReleaseSeatBooking(order.OrderItems.Select(x => x.SeatId));
            }

            await _eventsRepo.UpdateAsync(@event);

            await _ordersRepo.UpdateAsync(order);
        }

        return Unit.Value;
    }

    private async Task CreateTicketsAsync(OrderEntity order) 
    {
        foreach (var orderItem in order.OrderItems)
        {
            // TODO: consider redesign ticket entity:
            //  - remove status, use order status to check ticket status
            //  - consider make ticket as order's value object
            var ticket = new TicketEntity
            {
                Id = Guid.NewGuid(),
                EventId = order.EventId,
                OrderId = order.Id,
                SeatId = orderItem.SeatId,
                PriceId = orderItem.PriceId,
                Status = TicketStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ValidationHashCode = Guid.NewGuid().ToString()
            };

            await _ticketsRepo.AddAsync(ticket);
        }
    }
}