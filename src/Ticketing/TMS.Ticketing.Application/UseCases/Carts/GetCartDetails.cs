using TMS.Common.Users;

namespace TMS.Ticketing.Application.UseCases.Carts;

public sealed record GetCartDetails(Guid CartId) : IRequest<CartDetailsDto>, IValidatable 
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
        {
            x.RuleFor(y => y.CartId).NotEmpty();
        });
    }
};

internal sealed class GetCartDetailsHandler : IRequestHandler<GetCartDetails, CartDetailsDto>
{
    private readonly ICartsRepository _cartRepository;

    private readonly IUserContext _userContext;

    public GetCartDetailsHandler(ICartsRepository cartRepository, IUserContext userContext)
    {
        this._cartRepository = cartRepository;
        this._userContext = userContext;
    }

    public async Task<CartDetailsDto> Handle(GetCartDetails request, CancellationToken cancellationToken)
    {
        var user = _userContext.GetUser();

        var cart = await _cartRepository.GetRequiredAsync(request.CartId);

        if (cart.AccountId != user.Id)
        {
            throw ApiError.Forbidden("Cart does not belong to the user")
                .ToException();
        }

        return CartDetailsDto.Map(cart);
    }
}