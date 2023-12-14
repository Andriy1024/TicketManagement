namespace TMS.Ticketing.Application.Helpers;

public static class CacheKeys
{
    public static string GetVenueKey(Guid id) => $"venue_{id}";

    public static string GetEventKey(Guid id) => $"event_{id}";

    public static string GetVenueBookingKey(Guid id) => $"venue_booking_{id}";
}