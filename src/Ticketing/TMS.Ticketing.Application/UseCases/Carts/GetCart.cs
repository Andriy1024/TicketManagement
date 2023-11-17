namespace TMS.Ticketing.Application.UseCases.Carts;

public class GetCart : IRequest<CartDto>
{
    public Guid CartId { get; set; }
}

public class GetCartHandler : IRequestHandler<GetCart, CartDto>
{
    public Task<CartDto> Handle(GetCart request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}