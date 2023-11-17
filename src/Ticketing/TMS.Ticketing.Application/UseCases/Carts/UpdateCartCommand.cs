using TMS.Common.Errors;
using TMS.Common.Users;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Ordeting;

namespace TMS.Ticketing.Application.UseCases.Carts;

public class UpdateCartCommand : IRequest<CartDto>
{
    public Guid CartId { get; set; }

    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public Guid PriceId { get; set; }
}

public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, CartDto>
{
    private readonly IRepository<CartEntity, Guid> _cartRepository;
    private readonly IRepository<EventEntity, Guid> _eventsRepository;
    private readonly IUserContext _userContext;

    public UpdateCartHandler(
        IRepository<CartEntity, Guid> cartRepository, 
        IRepository<EventEntity, Guid> eventsRepository, 
        IUserContext userContext)
    {
        this._cartRepository = cartRepository;
        this._eventsRepository = eventsRepository;
        this._userContext = userContext;
    }

    public async Task<CartDto> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(request.CartId);

        if (cart == null)
        {
            cart = new CartEntity
            {
                Id = request.CartId,
                AccountId = _userContext.GetUser().Id
            };

            await _cartRepository.AddAsync(cart);
        }

        var @event = await _eventsRepository.GetAsync(request.EventId);

        if (@event == null) throw AppError
            .NotFound("Event was not found")
            .ToException();

        var seat = @event.Seats.Find(x => x.SeatId == request.SeatId);
        var price = @event.Prices.Find(x => x.Id == request.PriceId);
        var offer = @event.Offers.Find(x => x.SeatId == request.SeatId && x.PriceId == request.PriceId);

        AppError? validationError = (seat, price, offer) switch
        {
            { seat: null } => AppError.NotFound("Seat was not found"),
            { price: null } => AppError.NotFound("Price was not found"),
            { offer: null } => AppError.NotFound("Offer was not found"),
            { seat.State: not SeatState.Available } => AppError.NotFound("Seat is not available"),
            _ => null
        };

        if (validationError != null) throw validationError.ToException();

        var orderItem = new OrderItem
        {
            EventId = @event.Id,
            SeatId = seat!.SeatId,
            PriceId = price!.Id,
            Amount = price.Amount
        };

        cart.OrderItems.Add(orderItem);

        await _cartRepository.UpdateAsync(cart);

        return new CartDto();
    }
}