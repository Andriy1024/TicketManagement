namespace TMS.Ticketing.Infrastructure.Transactions;

public interface ITransactionManager
{
    Task<TResult> ExecInTransaction<TResult>(Func<Task<TResult>> func);
}