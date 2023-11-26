using FluentValidation.Results;

namespace TMS.Common.Validation;

public interface IValidatable
{
    IEnumerable<ValidationFailure> Validate();
}
