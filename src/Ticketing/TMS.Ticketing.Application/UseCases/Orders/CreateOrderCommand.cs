using TMS.Common.Extensions;
using TMS.Common.Users;
using TMS.Ticketing.Application.Interfaces;
using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Application.UseCases.Orders;

public record CreateOrderCommand(Guid CartId) : ICommand<CreateOrderCommandResult>, IValidatable 
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
            x.RuleFor(y => y.CartId).NotEmpty());
    }
};

public record CreateOrderCommandResult(Guid PaymentId);

internal sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderCommandResult>
{
    private readonly ICartsRepository _cartRepo;
    private readonly IEventsRepository _eventsRepo;
    private readonly IOrdersRepository _ordersRepo;
    private readonly IUserContext _userContext;
    private readonly IPaymentsService _payments;

    public CreateOrderHandler(
        ICartsRepository cartRepo, 
        IEventsRepository eventsRepo, 
        IOrdersRepository ordersRepo, 
        IUserContext userContext, 
        IPaymentsService payments)
    {
        _cartRepo = cartRepo;
        _eventsRepo = eventsRepo;
        _ordersRepo = ordersRepo;
        _userContext = userContext;
        _payments = payments;
    }

    public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var user = _userContext.GetUser();

        var cart = await _cartRepo.GetRequiredAsync(request.CartId);

        if (cart.AccountId != user.Id)
        {
            throw ApiError.Forbidden("Cart does not belong the the user").ToException();
        }

        if (cart.OrderItems.IsNullOrEmpty())
        {
            throw ApiError.InvalidData("Cart is empty").ToException();
        }

        var paymentId = Guid.NewGuid();

        foreach (var orderItems in cart.OrderItems.GroupBy(x => x.EventId))
        {
            var @event = await _eventsRepo.GetRequiredAsync(orderItems.Key);

            var order = OrderEntity.Create(user, @event, paymentId, orderItems);

            await _eventsRepo.UpdateAsync(@event);

            await _ordersRepo.AddAsync(order);
        }

        await _cartRepo.DeleteAsync(cart);

        await _payments.CreatePaymentAsync(paymentId, cart.Total, user.Id);

        return new CreateOrderCommandResult(paymentId);
    }
}