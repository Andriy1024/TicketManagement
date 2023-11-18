namespace TMS.Ticketing.Application.UseCases.Venues;

public sealed class DeleteVenueCommand : IRequest<Unit>
{
    public required Guid Id { get; init; }
}

internal sealed class DeleteVenueHandler : IRequestHandler<DeleteVenueCommand, Unit>
{
    private readonly IVenuesRepository _repository;

    public DeleteVenueHandler(IVenuesRepository repository)
    {
        this._repository = repository;
    }

    public async Task<Unit> Handle(DeleteVenueCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);

        return Unit.Value;
    }
}