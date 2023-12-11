using TMS.Common.Errors;

using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.DateRanges;
using TMS.Ticketing.Domain.DomainEvents;
using TMS.Ticketing.Domain.Venues;

namespace TMS.Ticketing.Domain.Events;

public sealed class EventEntity : Entity, IEntity<Guid>, IDateRange
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public List<Detail>? Details { get; set; }

    public required DateTime Start { get; set; }

    public required DateTime End { get; set; }

    public required int CreatorId { get; set; }

    public List<EventSeat> Seats { get; set; } = new();

    public List<Price> Prices { get; set; } = new();

    public List<Offer> Offers { get; set; } = new();

    public int Version { get; set; } = 1;

    public EventSeat GetSeat(Guid seatId) 
        => Seats.Find(x => x.SeatId == seatId)
            ?? throw ApiError
                .InvalidData($"Seat not found: {seatId}")
                .ToException();

    public Price GetPrice(Guid priceId)
        => Prices.Find(x => x.Id == priceId)
            ?? throw ApiError
                .InvalidData($"Price not found: {priceId}")
                .ToException();

    public Offer GetOffer(Guid seatId, Guid priceId)
        => Offers.Find(x => x.SeatId == seatId && x.PriceId == priceId)
            ?? throw ApiError
                .InvalidData($"Offer not found: Seat - {seatId}, Price - {priceId}")
                .ToException();

    public EventEntity Delete()
    {
        if (Seats.Any(x => x.State != SeatState.Available))
        {
            throw ApiError
                .InvalidData($"Event has seats that have been in use")
                .ToException();
        }

        AddDomainEvent(new EntityDeleted<EventEntity>(this));

        return this;
    }

    public EventEntity Update(
        string name, 
        List<Detail>? details,
        DateTime start,
        DateTime end)
    {
        Name = name;
        Details = details;

        // TODO: add validation to make sure new date range in scope of venue booking
        Start = start;
        End = end;

        AddDomainEvent(new EntityUpdated<EventEntity>(this));

        return this;
    }

    public EventEntity CreateSeats(VenueEntity venue)
    {
        if (Seats.Any(x => x.State != SeatState.Available))
        {
            throw ApiError
                .InternalServerError($"Events can't reset seats, because there are already seats in use")
                .ToException();
        }

        Seats = venue.Sections
            .SelectMany(x => x.Seats)
            .Select(x => new EventSeat
            {
                SeatId = x.SeatId,
                State = SeatState.Available
            })
            .ToList();

        AddDomainEvent(new EntityUpdated<EventEntity>(this));

        return this;
    }

    public EventEntity AddPrice(string name, decimal amount) 
    {
        Prices.Add(new Price
        {
            Id = Guid.NewGuid(),
            EventId = Id,
            Name = name,
            Amount = amount
        });

        AddDomainEvent(new EntityUpdated<EventEntity>(this));

        return this;
    }

    public EventEntity AddOffer(Guid priceId, Guid seatId)
    {
        _ = GetPrice(priceId);
        _ = GetSeat(seatId);

        Offers.Add(new Offer
        {
            PriceId = priceId,
            SeatId = seatId,
        });

        AddDomainEvent(new EntityUpdated<EventEntity>(this));

        return this;
    }

    public EventEntity BookSeat(Guid seatId) 
    {
        var eventSeat = GetSeat(seatId);

        var error = eventSeat.State switch
        {
            SeatState.Booked => ApiError.InvalidData($"Seat {seatId} already booked"),
            SeatState.Sold => ApiError.InvalidData($"Seat {seatId} already sold"),
            SeatState.Available => null,
            _ => ApiError.InternalServerError($"Unexpected seat state: {eventSeat.State}")
        };

        if (error != null) throw error.ToException();

        eventSeat.State = SeatState.Booked;

        AddDomainEvent(new EntityUpdated<EventEntity>(this));

        return this;
    }

    public EventEntity SellSeat(IEnumerable<Guid> seats)
    {
        foreach (var seatId in seats)
        {
            var eventSeat = GetSeat(seatId);

            var error = eventSeat.State switch
            {
                SeatState.Available => ApiError.InvalidData($"Seat {seatId} booking was cancelled"),
                SeatState.Sold => ApiError.InvalidData($"Seat {seatId} already sold"),
                SeatState.Booked => null,
                _ => ApiError.InternalServerError($"Unexpected seat state: {eventSeat.State}")
            };

            if (error != null) throw error.ToException();

            eventSeat.State = SeatState.Sold;
        }

        AddDomainEvent(new EntityUpdated<EventEntity>(this));

        return this;
    }

    public EventEntity ReleaseSeatBooking(IEnumerable<Guid> seats)
    {
        foreach (var seatId in seats)
        {
            var eventSeat = GetSeat(seatId);

            var error = eventSeat.State switch
            {
                SeatState.Available => ApiError.InvalidData($"Seat {seatId} is already available"),
                SeatState.Sold => ApiError.InvalidData($"Seat {seatId} already sold"),
                SeatState.Booked => null,
                _ => ApiError.InternalServerError($"Unexpected seat state: {eventSeat.State}")
            };

            if (error != null) throw error.ToException();

            eventSeat.State = SeatState.Available;
        }

        AddDomainEvent(new EntityUpdated<EventEntity>(this));

        return this;
    }

    public (int Old, int New) IncreaseVersion() => (Version, ++Version);
}