﻿using TMS.Common.Errors;
using TMS.Common.Users;

namespace TMS.Ticketing.Application.UseCases.Carts;

public sealed class GetCartDetails : IRequest<CartDetailsDto>
{
    public Guid CartId { get; set; }
}

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
            throw AppError.Forbidden("Cart does not belong to the user")
                .ToException();
        }

        return CartDetailsDto.Map(cart);
    }
}