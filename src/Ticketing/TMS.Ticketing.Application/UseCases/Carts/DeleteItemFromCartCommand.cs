namespace TMS.Ticketing.Application.UseCases.Carts;

public sealed class DeleteItemFromCartCommand : IRequest<CartDetailsDto>, IValidatable
{
    public required Guid CartId { get; init; }

    public required Guid EventId { get; init; }

    public required Guid SeatId { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.CartId).NotEmpty();
            x.RuleFor(y => y.EventId).NotEmpty();
            x.RuleFor(y => y.SeatId).NotEmpty();
        });
    }
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
                ?? throw ApiError.NotFound("Cart item not found").ToException();

        cart.OrderItems.Remove(orderItem);

        return CartDetailsDto.Map(cart);
    }
}