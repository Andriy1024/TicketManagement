﻿namespace TMS.Ticketing.Application.UseCases.Venues;

public sealed record DeleteVenueCommand(Guid Id) : ICommand<Unit>, IValidatable
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
            x.RuleFor(y => y.Id).NotEmpty());
    }
}

internal sealed class DeleteVenueHandler : IRequestHandler<DeleteVenueCommand, Unit>
{
    private readonly IVenuesRepository _repository;

    public DeleteVenueHandler(IVenuesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteVenueCommand request, CancellationToken cancellationToken)
    {
        var venue = await _repository.GetRequiredAsync(request.Id);

        venue.Delete();

        await _repository.DeleteAsync(venue);

        return Unit.Value;
    }
}