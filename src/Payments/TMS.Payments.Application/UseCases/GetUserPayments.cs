using MediatR;

using TMS.Payments.Application.Interfaces;
using TMS.Payments.Domain.Views;

namespace TMS.Payments.Application.UseCases;

public sealed record GetUserPayments(int UserId) : IRequest<UserPaymentsView>
{
}

internal sealed class GetUserPaymentsHandler : IRequestHandler<GetUserPayments, UserPaymentsView>
{
    private readonly IPaymentsViewRepository _repository;

    public GetUserPaymentsHandler(IPaymentsViewRepository repositpry)
    {
        _repository = repositpry;
    }

    public async Task<UserPaymentsView> Handle(GetUserPayments request, CancellationToken cancellationToken)
    {
        var userPayments = await _repository.GetUserPaymentsAsync(request.UserId, cancellationToken);

        return userPayments ?? new UserPaymentsView { AccountId = request.UserId };
    }
}