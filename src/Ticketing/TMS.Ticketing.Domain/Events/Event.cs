using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Domain.Events;

public sealed class Event
{
    public required int EventId { get; set; }
    
    public required string Name { get; set; }

    public required List<Detail> Details { get; set; }

    public required VenueBooking Venue { get; set; }

    public required DateTime Start { get; set; }
    
    public required DateTime End { get; set; }

    public required List<EventManager> Managers { get; set; }

    public required List<EventSeat> Seats { get; set; }

    public required List<PriceType> Prices { get; set; }

    public required List<Offer> Offers { get; set; }
}