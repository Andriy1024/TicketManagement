using TMS.Common.Errors;
using TMS.Ticketing.Domain.Common;
using TMS.Ticketing.Domain.DomainEvents;

namespace TMS.Ticketing.Domain.Venues;

public sealed class VenueEntity : Entity<Guid>
{
    public required string Name { get; set; }
    
    public required string Country { get; set; }
    
    public required string City { get; set; }
    
    public required string Street { get; set; }

    public List<Detail>? Details { get; set; }

    public List<VenueSection> Sections { get; set; } = new();

    public VenueSection GetSection(Guid sectionId)
    {
        return Sections.Find(x => x.SectionId == sectionId) 
            ?? throw ApiError.NotFound($"Section not found: {sectionId}").ToException();
    }

    public VenueEntity Delete() 
    {
        AddDomainEvent(new EntityDeleted<VenueEntity>(this));

        return this;
    }

    public VenueEntity CreateSection(string name, SectionType type) 
    {
        Sections.Add(new VenueSection
        {
            SectionId = Guid.NewGuid(),
            VenueId = Id,
            Name = name,
            Type = type,
        });

        AddDomainEvent(new EntityUpdated<VenueEntity>(this));

        return this;
    }

    public VenueEntity DeleteSection(Guid sectionId)
    {
        var section = GetSection(sectionId);

        Sections.Remove(section);

        AddDomainEvent(new EntityUpdated<VenueEntity>(this));

        return this;
    }

    public VenueEntity UpdateSection(Guid sectionId, string name, SectionType type)
    {
        var section = GetSection(sectionId);

        section.Name = name;
        section.Type = type;

        AddDomainEvent(new EntityUpdated<VenueEntity>(this));

        return this;
    }

    public VenueEntity CreateSeat(Guid sectionId, int? rowNumber)
    {
        var section = GetSection(sectionId);

        var rowSeats = section.Seats
            .Where(x => x.RowNumber == rowNumber)
            .ToArray();

        var newSeatNumber = rowSeats.Length == 0 ? 1
            : section.Seats[rowSeats.Length - 1].SeatNumber + 1;

        var seat = new VenueSeat
        {
            SeatId = Guid.NewGuid(),
            SectionId = section.SectionId,
            RowNumber = rowNumber,
            SeatNumber = newSeatNumber
        };

        section.Seats.Add(seat);

        AddDomainEvent(new EntityUpdated<VenueEntity>(this));

        return this;
    }

    public VenueEntity DeleteSeat(Guid sectionId, Guid seatId)
    {
        var section = GetSection(sectionId);

        var seat = section.GetSeat(seatId);

        section.Seats.Remove(seat);

        var rowSeat = section.Seats
            .Where(x => x.RowNumber == seat.RowNumber)
            .ToArray();

        for (int i = 0; i < rowSeat.Length; i++)
        {
            rowSeat[i].SeatNumber = i + 1;
        }

        AddDomainEvent(new EntityUpdated<VenueEntity>(this));

        return this;
    }
}