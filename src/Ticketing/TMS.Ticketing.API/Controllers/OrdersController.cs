using Microsoft.AspNetCore.Mvc;

using MediatR;

using TMS.Ticketing.Application.UseCases.Carts;
using TMS.Ticketing.Application.Dtos;

namespace TMS.Ticketing.API.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    #region Cart

    [HttpGet("carts/{cartId}")]
    public Task<CartDto> GetCartAsync([FromRoute] Guid cartId, CancellationToken token)
        => _mediator.Send(new GetCart { CartId = cartId });

    [HttpPost("carts")]
    public Task<CartDto> UpdateCartAsync([FromBody] UpdateCartCommand command)
        => _mediator.Send(command);

    #endregion

    #region Order

    // TODO: Create Order CRUD

    #endregion
}
