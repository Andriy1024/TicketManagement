using FluentValidation;
using FluentValidation.Results;

using TMS.Common.Enums;
using TMS.Common.Validation;

namespace TMS.Common.IntegrationEvents;

public class PaymentStatusUpdated : IValidatable
{
    public Guid PaymentId { get; set; }

    public PaymentStatus Status { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x => 
        {
            x.RuleFor(x => PaymentId).NotEmpty();
            x.RuleFor(x => Status).IsInEnum();
        });
    }
}