using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TMS.Notifications.Application.UseCases;

namespace TMS.Notifications.Infrastructure.BackgroundServices;

internal sealed class NotificationsBackgroundService : BackgroundService
{
    private static readonly TimeSpan _delay = TimeSpan.FromSeconds(10);

    private readonly ILogger<NotificationsBackgroundService> _logger;
    
    private readonly IServiceScopeFactory _scopeFactory;

    public NotificationsBackgroundService(ILogger<NotificationsBackgroundService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new SendNotificationsCommand(), CancellationToken.None);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            await Task.Delay(_delay, stoppingToken);
        }
    }
}