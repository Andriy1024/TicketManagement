using MediatR;

using TMS.Common.Interfaces;

namespace TMS.Ticketing.Infrastructure.Transactions;

internal sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ITransactionManager _transactionManager;

    public TransactionBehavior(ITransactionManager sessionFactory)
    {
        _transactionManager = sessionFactory;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICommand<TResponse>)
        {
            return next();
        }

        return _transactionManager.ExecInTransaction(() => next());
    }
}