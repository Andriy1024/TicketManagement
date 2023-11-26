using MediatR;

namespace TMS.Common.Validation;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IValidatable validatableRequest)
        {
            validatableRequest
                .Validate()
                .ThrowIfInvalid();
        }

        return next();
    }
}
