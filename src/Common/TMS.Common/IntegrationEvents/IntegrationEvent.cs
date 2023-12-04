using FluentValidation;
using FluentValidation.Results;

using MediatR;

using TMS.Common.Validation;

namespace TMS.Common.IntegrationEvents;

public class IntegrationEvent<TPayload> : IRequest<Unit>, IValidatable
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
            result.Concat(validatable.Validate());
        }

        return result;
    }
}