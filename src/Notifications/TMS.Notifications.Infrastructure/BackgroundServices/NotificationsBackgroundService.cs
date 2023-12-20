using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TMS.Notifications.Application.Interfaces;
using TMS.Notifications.Application.UseCases;
using TMS.Notifications.Domain.Enums;

namespace TMS.Notifications.Infrastructure.BackgroundServices;

internal class NotificationsBackgroundService : BackgroundService
{
    private static readonly TimeSpan MinimumMessageAgeToBatch = TimeSpan.FromSeconds(30);
    private readonly ILogger<NotificationsBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public NotificationsBackgroundService(ILogger<NotificationsBackgroundService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            DateTime minimumMessageAgeToBatch = DateTime.UtcNow.Subtract(MinimumMessageAgeToBatch);

            try
            {
                using var scope = _scopeFactory.CreateScope();

                var repo = scope.ServiceProvider.GetRequiredService<INotificationsRepository>();

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var notification = await repo.GetAsync(x => x.Status == NotificationStatus.Pending);

                if (notification != null)
                {
                    await mediator.Send(new ProccessNotification(notification), CancellationToken.None);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            await Task.Delay(MinimumMessageAgeToBatch, stoppingToken);
        }
    }
}