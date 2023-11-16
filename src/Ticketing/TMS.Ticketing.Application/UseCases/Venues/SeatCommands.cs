namespace TMS.Ticketing.Application.UseCases.Venues;

public class CreateSeatCommand : IRequest<VenueDetailsDto>
{
    public required Guid VenueId { get; init; }

    public required Guid SectionId { get; init; }

    public required int? RowNumber { get; init; }
}

public class DeleteSeatCommand : IRequest<VenueDetailsDto>
{
    public required Guid VenueId { get; init; }

    public required Guid SectionId { get; init; }

    public required Guid SeatId { get; init; }
}