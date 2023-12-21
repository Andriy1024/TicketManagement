using FluentValidation;
using FluentValidation.Results;

using MediatR;

using TMS.Common.Interfaces;
using TMS.Common.Validation;

namespace TMS.Common.IntegrationEvents;

/// <summary>
/// The interface represents an event that is sent to message broker (RabbitMq).
/// </summary>
public interface IIntegrationEvent : ICommand<Unit>
{
}

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