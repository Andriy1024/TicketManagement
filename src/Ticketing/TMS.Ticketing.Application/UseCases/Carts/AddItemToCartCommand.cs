using TMS.Common.Users;
using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Application.UseCases.Carts;

public sealed class AddItemToCartCommand : ICommand<CartDetailsDto>, IValidatable
{
    public Guid CartId { get; set; }

    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public Guid PriceId { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x => 
        {
            x.RuleFor(y => y.CartId).NotEmpty();
            x.RuleFor(y => y.EventId).NotEmpty();
            x.RuleFor(y => y.SeatId).NotEmpty();
            x.RuleFor(y => y.PriceId).NotEmpty();
        });
    }
}

internal class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, CartDetailsDto>
{
    private readonly ICartsRepository _cartRepo;
    private readonly IEventsRepository _eventsRepo;
    private readonly IUserContext _userContext;

    public AddItemToCartHandler(
        ICartsRepository cartRepo,
        IEventsRepository eventsRepo, 
        IUserContext userContext)
    {
        _cartRepo = cartRepo;
        _eventsRepo = eventsRepo;
        _userContext = userContext;
    }

    public async Task<CartDetailsDto> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepo.GetAsync(request.CartId);

        bool isNewCart = cart == null;

        cart ??= new CartEntity
        {
            Id = request.CartId,
            AccountId = _userContext.GetUser().Id
        };

        var @event = await _eventsRepo.GetRequiredAsync(request.EventId);

        cart.AddItem(@event, request.SeatId, request.PriceId);

        if (isNewCart) 
            await _cartRepo.AddAsync(cart);
        else
            await _cartRepo.UpdateAsync(cart);

        return CartDetailsDto.Map(cart);
    }
}