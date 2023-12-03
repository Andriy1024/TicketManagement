namespace TMS.Ticketing.Domain.DateRanges;

public static class DateRangeExtensions
{
    public static bool IsDateRangeAvailable(this IEnumerable<IDateRange> bookings,
        DateTime startDate, DateTime endDate)
    {
        // Check if there are no bookings that overlap with the specified date range
        bool isAvailable = !bookings.Any(booking =>
            (startDate >= booking.Start && startDate <= booking.End) ||
            (endDate >= booking.Start && endDate <= booking.End) ||
            (startDate <= booking.Start && endDate >= booking.End));

        return isAvailable;
    }

    public static bool IsInRange(this IDateRange dateRange,
        DateTime startDate, DateTime endDate)
    {
        return
            startDate >= dateRange.Start &&
            startDate <= dateRange.End &&
            endDate <= dateRange.End;
    }
}