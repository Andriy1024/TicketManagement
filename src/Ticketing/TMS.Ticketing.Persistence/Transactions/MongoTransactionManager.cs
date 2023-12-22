using TMS.Ticketing.Infrastructure.Transactions;

namespace TMS.Ticketing.Persistence.Sessions;

internal sealed class MongoTransactionManager : ITransactionManager
{
    private readonly MongoTransactionScopeFactory _sessionFactory;

    public MongoTransactionManager(MongoTransactionScopeFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }

    public async Task<TResult> ExecInTransaction<TResult>(Func<Task<TResult>> func)
    {
        var session = await _sessionFactory.CreateAsync();

        session.StartTransaction();

        try
        {
            var result = await func();

            await session.CommitAsync();

            return result;
        }
        catch (Exception)
        {
            await session.AbortAsync();

            throw;
        }
        finally
        {
            session?.Dispose();
        }
    }
}