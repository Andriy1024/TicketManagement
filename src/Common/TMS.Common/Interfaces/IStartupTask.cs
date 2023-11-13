namespace TMS.Common.Interfaces;

public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}