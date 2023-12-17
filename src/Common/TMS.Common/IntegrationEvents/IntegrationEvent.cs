using FluentValidation;
using FluentValidation.Results;

using MediatR;

using TMS.Common.Validation;

namespace TMS.Common.IntegrationEvents;

public class IntegrationEvent<TPayload> : IIntegrationEvent, IValidatable
{
    public TPayload Payload { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        var result = this.Validate(x =>
        {
            x.RuleFor(x => x.Payload).NotNull();
        });

        if (Payload is IValidatable validatable)
        {
            result = result.Concat(validatable.Validate());
        }

        return result;
    }
}

public interface IHasTransaction
{
}

/// <summary>
/// The interface represents an event that is sent to message broker (RabbitMq).
/// </summary>
public interface IIntegrationEvent : IRequest<Unit>
{
}

/// <summary>
/// Persistent events are saved to database before publish to message broker.
/// </summary>
public interface IPersistentIntegrationEvent : IIntegrationEvent, IHasTransaction
{
    public Guid Id { get; set; }
}

/// <summary>
/// Transient events are not persistent in database.
/// </summary>
public interface ITransientIntegrationEvent : IIntegrationEvent
{
}