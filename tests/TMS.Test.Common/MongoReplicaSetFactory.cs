using EphemeralMongo;

using Xunit;

namespace TMS.Test.Common;

public class MongoReplicaSetFactory : IAsyncLifetime
{
    private IMongoRunner? _mongoDb;

    private readonly MongoRunnerOptions _options;

    public string ConnectionString => _mongoDb?.ConnectionString
        ?? throw new InvalidOperationException(nameof(ConnectionString));

    public MongoReplicaSetFactory()
    {
        _options = new MongoRunnerOptions
        {
            UseSingleNodeReplicaSet = true, // Default: false
            StandardOuputLogger = line => Console.WriteLine(line), // Default: null
            StandardErrorLogger = line => Console.WriteLine(line), // Default: null
            ConnectionTimeout = TimeSpan.FromSeconds(10), // Default: 30 seconds
            ReplicaSetSetupTimeout = TimeSpan.FromSeconds(5), // Default: 10 seconds
            // EXPERIMENTAL - Only works on Windows and modern .NET (netcoreapp3.1, net5.0, net6.0, net7.0 and so on):
            // Ensures that all MongoDB child processes are killed when the current process is prematurely killed,
            // for instance when killed from the task manager or the IDE unit tests window. Processes are managed as a unit using
            // job objects: https://learn.microsoft.com/en-us/windows/win32/procthread/job-objects
            KillMongoProcessesWhenCurrentProcessExits = true // Default: false
        };
    }

    public Task InitializeAsync()
    {
        _mongoDb = MongoRunner.Run(_options);

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _mongoDb?.Dispose();

        return Task.CompletedTask;
    }
}