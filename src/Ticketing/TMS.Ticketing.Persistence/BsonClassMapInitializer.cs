using MongoDB.Bson.Serialization;

using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Tickets;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Persistence;

public static class BsonClassMapInitializer
{
    private static object _lock = new object();

    private static bool IsInitialize = false;

    public static void Initialize()
    {
        lock (_lock)
        {
            if (IsInitialize)
            {
                return;
            }

            IsInitialize = true;
        }

        BsonClassMap.RegisterClassMap<VenueEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<VenueBookingEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<EventEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<CartEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
            map.UnmapProperty(x => x.Total);
        });

        BsonClassMap.RegisterClassMap<OrderEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<TicketEntity>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id);
        });
    }
}