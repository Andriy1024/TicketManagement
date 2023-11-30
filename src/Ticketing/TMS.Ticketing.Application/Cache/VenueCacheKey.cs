namespace TMS.Ticketing.Application.Cache;

public static class VenueCacheKey
{
    public static string GetKey(Guid id) => $"venue_{id}";
}
