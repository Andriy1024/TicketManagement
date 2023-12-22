namespace TMS.Ticketing.Persistence.Sessions;

internal sealed class MongoTransactionScopeFactory
{
    private readonly IMongoClient _client;

    private readonly MongoTransactionScope _transactionScope;

    public MongoTransactionScopeFactory(IMongoClient client, MongoTransactionScope transactionScope)
    {
        _client = client;
        _transactionScope = transactionScope;
    }

    public async Task<MongoTransactionScope> CreateAsync()
    {
        if (_transactionScope.HasTransaction)
        {
            throw new InvalidOperationException("Transaction already exists");
        }

        var session = await _client.StartSessionAsync();

        _transactionScope.AddTransaction(session);

        return _transactionScope;
    }
}