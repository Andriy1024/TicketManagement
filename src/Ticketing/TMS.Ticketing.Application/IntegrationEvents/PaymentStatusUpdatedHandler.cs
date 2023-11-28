using TMS.Common.Enums;
using TMS.Common.IntegrationEvents;

using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordeting;
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

        var newOrderStatus = payload.Status switch
        {
            PaymentStatus.Completed => OrderStatus.Completed,
            PaymentStatus.Failed => OrderStatus.Failed,
            _ => throw new NotImplementedException($"Unexpected Payment Status: {payload.Status}")
        };

        var newSeatState = newOrderStatus switch 
        {
            OrderStatus.Completed => SeatState.Sold,
            OrderStatus.Failed => SeatState.Available,
            _ => throw new NotImplementedException($"Unexpected Order Status: {newOrderStatus}")
        };

        foreach (var order in orders) 
        {
            order.Status = newOrderStatus;

            var @event = await _eventsRepo.GetRequiredAsync(order.EventId);

            foreach (var orderItem in order.OrderItems)
            {
                var eventSeat = @event.GetSeat(orderItem.SeatId);

                eventSeat.State = newSeatState;

                if (newSeatState == SeatState.Sold)
                {
                    // TODO: consider redesign ticket entity:
                    //  - remove status, use order status to check ticket status
                    //  - consider make ticket as order's value object
                    var ticket = new TicketEntity
                    {
                        Id = Guid.NewGuid(),
                        EventId = @event.Id,
                        OrderId = order.Id,
                        SeatId = eventSeat.SeatId,
                        PriceId = orderItem.PriceId,
                        Status = TicketStatus.Pending,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        ValidationHashCode = Guid.NewGuid().ToString()
                    };

                    await _ticketsRepo.AddAsync(ticket);
                }
            }

            await _eventsRepo.UpdateAsync(@event);

            await _ordersRepo.UpdateAsync(order);
        }

        return Unit.Value;
    }
}