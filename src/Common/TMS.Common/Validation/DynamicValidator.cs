using FluentValidation;

namespace TMS.Common.Validation;

public class DynamicValidator<T> : AbstractValidator<T>
{
    public DynamicValidator(Action<DynamicValidator<T>> configuration)
    {
        configuration(this);
    }
}
