using TMS.Ticketing.Domain.Common;

namespace TMS.Ticketing.API.Dtos.Events;

public class EventOverviewDto : EventPropertiesDto
{
    public Guid Id { get; set; }
}

public class EventPropertiesDto
{
    public string Name { get; set; }

    public List<KeyValePair>? Details { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}