namespace TMS.Ticketing.Persistence.Sessions;

internal sealed class MongoTransactionScope 
{
    private IClientSessionHandle? _transaction = null;

    public bool HasTransaction => _transaction != null;

    public IClientSessionHandle GetTransaction()
    {
        if (!HasTransaction)
            throw new InvalidOperationException("Transaction is empty");

        return _transaction!;
    }

    public void AddTransaction(IClientSessionHandle transaction)
    {
        if (HasTransaction)
            throw new InvalidOperationException("Transaction already exists");

        _transaction = transaction;
    }

    public void StartTransaction()
    {
        GetTransaction().StartTransaction();
    }

    public Task CommitAsync()
    {
        return GetTransaction().CommitTransactionAsync();
    }

    public Task AbortAsync()
    {
        return GetTransaction().AbortTransactionAsync();
    }

    public void Dispose()
    {
        var transaction = GetTransaction();

        transaction.Dispose();

        _transaction = null;
    }
}