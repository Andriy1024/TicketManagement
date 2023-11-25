using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Events;

namespace TMS.Ticketing.Application.Dtos;

public class EventOverviewDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<Detail>? Details { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public static EventOverviewDto Map(EventEntity @event) => new() 
    {
        Id = @event.Id,
        Name = @event.Name,
        Details = @event.Details,
        Start = @event.Start,
        End = @event.End
    };
}