using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.Dtos;

public class PriceDto 
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public string Name { get; set; }

    public static PriceDto Map(Price price) => new() 
    { 
        Id = price.Id,
        Name = price.Name, 
        Amount = price.Amount 
    };
}