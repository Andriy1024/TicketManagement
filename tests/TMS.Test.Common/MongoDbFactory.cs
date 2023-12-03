using Testcontainers.MongoDb;

using Xunit;

namespace TMS.Test.Common;

public class MongoDBFactory : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDb;

    public string ConnectionString => _mongoDb.GetConnectionString();

    public MongoDBFactory()
    {
        var dockerHost = Environment.GetEnvironmentVariable("DOCKER_HOST");

        var mongoBuilder = new MongoDbBuilder();

        if (!string.IsNullOrWhiteSpace(dockerHost))
        {
            mongoBuilder.WithHostname(dockerHost);
        }

        _mongoDb = mongoBuilder.Build();
    }

    public Task InitializeAsync()
        => _mongoDb.StartAsync();

    public Task DisposeAsync()
        => _mongoDb.DisposeAsync().AsTask();
}