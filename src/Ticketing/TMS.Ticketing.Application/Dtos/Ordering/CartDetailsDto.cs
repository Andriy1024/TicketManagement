using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Application.Dtos;

public class CartDetailsDto
{
    public Guid Id { get; set; }

    public int AccountId { get; set; }

    public decimal Total { get; set; }

    public IEnumerable<OrderItemDto> OrderItems { get; set; }

    public static CartDetailsDto Map(CartEntity cart) => new()
    {
        Id = cart.Id,
        AccountId = cart.AccountId,
        Total = cart.Total,
        OrderItems = cart.OrderItems.Select(OrderItemDto.Map).ToList()
    };
}