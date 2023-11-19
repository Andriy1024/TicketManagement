using MediatR;

using TMS.Common.Users;
using TMS.Payments.Domain.Abstractions;
using TMS.Payments.Domain.Views;

namespace TMS.Payments.Application.UseCases;

public sealed record GetUserPayments() : IRequest<UserPaymentsView>;

internal sealed class GetUserPaymentsHandler : IRequestHandler<GetUserPayments, UserPaymentsView>
{
    private readonly IUserContext _userContext;
    private readonly IPaymentsViewRepository _repository;

    public GetUserPaymentsHandler(IUserContext userContext, IPaymentsViewRepository repositpry)
    {
        _userContext = userContext;
        _repository = repositpry;
    }

    public async Task<UserPaymentsView> Handle(GetUserPayments request, CancellationToken cancellationToken)
    {
        var user = _userContext.GetUser();

        var userPayments = await _repository.GetUserPaymentsAsync(user.Id, cancellationToken);

        return userPayments ?? new UserPaymentsView { AccountId = user.Id };
    }
}