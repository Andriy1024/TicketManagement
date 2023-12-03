using TMS.Common.Users;

namespace TMS.Ticketing.Application.UseCases.Carts;

public sealed class GetUserCarts : IQuery<IEnumerable<UserCartDto>>
{
}

internal sealed class GetUserCartsHandler : IRequestHandler<GetUserCarts, IEnumerable<UserCartDto>>
{
    private readonly ICartsRepository _cartsRepository;
    
    private readonly IUserContext _userContext;

    public GetUserCartsHandler(ICartsRepository cartsRepository, IUserContext userContext)
    {
        _cartsRepository = cartsRepository;
        _userContext = userContext;
    }

    public async Task<IEnumerable<UserCartDto>> Handle(GetUserCarts request, CancellationToken cancellationToken)
    {
        var user = _userContext.GetUser();

        var carts = await _cartsRepository.FindAsync(x => x.AccountId == user.Id);

        return carts.Select(x => new UserCartDto
        {
            CartId = x.Id,
            AccountId = x.AccountId
        }).ToList();
    }
}