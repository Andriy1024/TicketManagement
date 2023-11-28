using Microsoft.AspNetCore.Mvc;

using MediatR;

using TMS.Ticketing.Application.UseCases.Carts;
using TMS.Ticketing.Application.Dtos;
using TMS.Ticketing.Application.UseCases.Orders;

namespace TMS.Ticketing.API.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
        => _mediator = mediator;

    #region Cart

    [HttpGet("carts")]
    public Task<IEnumerable<UserCartDto>> GetUserCarts(CancellationToken token)
        => _mediator.Send(new GetUserCarts(), token);

    [HttpGet("carts/{cartId}")]
    public Task<CartDetailsDto> GetCartDetailsAsync([FromRoute] Guid cartId, CancellationToken token)
        => _mediator.Send(new GetCartDetails(cartId));

    [HttpPost("carts")]
    public Task<CartDetailsDto> AddItemToCartAsync([FromBody] AddItemToCartCommand command)
        => _mediator.Send(command);

    [HttpDelete("carts/{cartId}/events/{eventId}/seats/{seatId}")]
    public Task<CartDetailsDto> DeleteItemFromCartAsync(
        [FromRoute] Guid cartId, [FromRoute] Guid eventId, [FromRoute] Guid seatId)
        => _mediator.Send(new DeleteItemFromCartCommand 
        {
            CartId = cartId,
            EventId = eventId,
            SeatId = seatId
        });

    #endregion

    #region Order

    [HttpPost("carts/{cartId}/book")]
    public Task<CreateOrderCommandResult> CreateOrderAsync([FromRoute] Guid cartId)
        => _mediator.Send(new CreateOrderCommand(cartId));

    #endregion
}
