namespace TMS.Ticketing.API.Dtos.Carts;

public class UpdateCartDto
{
    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public Guid PriceId { get; set; }
}
