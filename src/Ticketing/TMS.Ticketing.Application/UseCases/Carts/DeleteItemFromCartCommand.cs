
using TMS.Common.Errors;

namespace TMS.Ticketing.Application.UseCases.Carts;

public sealed class DeleteItemFromCartCommand : IRequest<CartDetailsDto>
{
    public required Guid CartId { get; init; }

    public required Guid EventId { get; init; }

    public required Guid SeatId { get; init; }
}

internal sealed class DeleteItemFromCartHandler : IRequestHandler<DeleteItemFromCartCommand, CartDetailsDto>
{
    private readonly ICartsRepository _cartRepo;

    public DeleteItemFromCartHandler(ICartsRepository cartRepo)
    {
        this._cartRepo = cartRepo;
    }

    public async Task<CartDetailsDto> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepo.GetRequiredAsync(request.CartId);

        var orderItem = cart.OrderItems
            .Find(x => x.EventId == request.EventId && x.SeatId == request.SeatId)
                ?? throw AppError.NotFound("Cart item not found").ToException();

        cart.OrderItems.Remove(orderItem);

        return CartDetailsDto.Map(cart);
    }
}