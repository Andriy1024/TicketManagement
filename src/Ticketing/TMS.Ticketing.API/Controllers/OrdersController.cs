using Microsoft.AspNetCore.Mvc;

using TMS.Common.Errors;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordeting;
using TMS.Ticketing.Persistence.Abstractions;
using TMS.Ticketing.API.Dtos.Carts;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TMS.Common.Users;

namespace TMS.Ticketing.API.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMongoRepository<Cart, Guid> _cartRepository;
    private readonly IMongoRepository<Order, Guid> _orderRepository;
    private readonly IMongoRepository<Event, Guid> _eventsRepository;
    private readonly IUserContext _userContext;

    public OrdersController(
        IMongoRepository<Cart, Guid> cartRepository,
        IMongoRepository<Order, Guid> orderRepository,
        IMongoRepository<Event, Guid> eventsRepository,
        IUserContext userContext)
    {
        this._cartRepository = cartRepository;
        this._orderRepository = orderRepository;
        this._eventsRepository = eventsRepository;
        this._userContext = userContext;
    }

    #region Cart

    [HttpGet("carts/{cartId}")]
    public async Task<IActionResult> GetCartAsync([FromRoute] Guid cartId, CancellationToken token)
    {
        var cart = await _cartRepository
            .Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == cartId, token);

        return Ok(cart);
    }

    [HttpPost("carts/{cartId}")]
    public async Task<IActionResult> UpdateCartAsync(
        [FromRoute] Guid cartId, 
        [FromBody] UpdateCartDto dto) 
    {
        var cart = await _cartRepository.GetAsync(cartId);

        if (cart == null) 
        {
            cart = new Cart
            {
                Id = cartId,
                AccountId = _userContext.GetUser().Id
            };

            await _cartRepository.AddAsync(cart);
        }
        
        var @event = await _eventsRepository.GetAsync(dto.EventId);

        if (@event == null) throw AppError
            .NotFound("Event was not found")
            .ToException();
        
        var seat = @event.Seats.Find(x => x.SeatId == dto.SeatId);
        var price = @event.Prices.Find(x => x.Id == dto.PriceId);
        var offer = @event.Offers.Find(x => x.SeatId == dto.SeatId && x.PriceId == dto.PriceId);

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

        return Ok(cart);
    }

    #endregion

    #region Order

    // TODO: Create Order CRUD

    #endregion
}
