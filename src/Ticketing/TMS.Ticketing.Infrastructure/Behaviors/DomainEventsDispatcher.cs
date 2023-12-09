using MediatR;

using TMS.Common.Interfaces;
using TMS.Ticketing.Infrastructure.ChangeTracker;

namespace TMS.Ticketing.Infrastructure.DomainEvents;

internal sealed class DomainEventsDispatcher<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEntityChangeTracker _domainEvents;
    private readonly IMediator _mediator;

    public DomainEventsDispatcher(IEntityChangeTracker domainEvents, IMediator mediator)
    {
        _domainEvents = domainEvents;
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if (request is ICommand<TResponse>)
        {
            foreach (var domainEvent in _domainEvents.ExtractEvents())
            {
                await _mediator.Publish(domainEvent);
            }
        }

        return result;
    }
}