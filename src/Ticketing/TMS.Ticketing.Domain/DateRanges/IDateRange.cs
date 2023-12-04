namespace TMS.Ticketing.Domain.DateRanges;

public interface IDateRange
{
    DateTime Start { get; }

    DateTime End { get; }
}