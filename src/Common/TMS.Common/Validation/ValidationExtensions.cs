using FluentValidation.Results;
using FluentValidation;
using TMS.Common.Errors;

namespace TMS.Common.Validation;

public static class ValidationExtensions
{
    public static IEnumerable<ValidationFailure> Validate<TObject>(this TObject obj, IEnumerable<IValidator<TObject>> validators)
    {
        var context = new ValidationContext<TObject>(obj);

        return validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToList();
    }

    public static IEnumerable<ValidationFailure> Validate<TObject>(this TObject obj, Action<DynamicValidator<TObject>> configuration)
    {
        return obj.Validate(new[] { new DynamicValidator<TObject>(configuration) });
    }

    public static string BuildErrorMessage(this IEnumerable<ValidationFailure> failures)
    {
        return string.Join(string.Empty, failures.Select(x => $"{Environment.NewLine} -- {x.PropertyName}: {x.ErrorMessage}"));
    }

    public static void ThrowIfInvalid(this IEnumerable<ValidationFailure> failures)
    {
        if (failures.Any())
        {
            var stringErrors = failures.BuildErrorMessage();

            throw ApiError
                    .InvalidData(message: stringErrors)
                    .ToException();
        }
    }

    public static void ThrowIfInvalid<TObject>(this TObject obj, IEnumerable<IValidator<TObject>> validators)
    {
        Validate(obj, validators).ThrowIfInvalid();
    }

    public static void ThrowIfInvalid<TObject>(this TObject obj, Action<DynamicValidator<TObject>> configuration)
    {
        obj.ThrowIfInvalid(new[] { new DynamicValidator<TObject>(configuration) });
    }
}