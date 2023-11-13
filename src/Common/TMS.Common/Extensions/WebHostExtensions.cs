using Microsoft.Extensions.DependencyInjection;

using TMS.Common.Interfaces;

namespace TMS.Common.Extensions;

public static class WebHostExtensions
{
    public static async Task RunStartupTasksAsync(this IServiceProvider services)
    {
        await using (var scope = services.CreateAsyncScope())
        {
            var startupTasks = scope.ServiceProvider.GetServices<IStartupTask>();

            foreach (var startupTask in startupTasks)
            {
                await startupTask.ExecuteAsync();
            }
        }
    }
}