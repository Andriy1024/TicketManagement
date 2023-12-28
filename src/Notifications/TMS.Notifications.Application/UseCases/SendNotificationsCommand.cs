using MediatR;

using Microsoft.Extensions.Logging;

using TMS.Common.Interfaces;
using TMS.Notifications.Application.Interfaces;
using TMS.Notifications.Domain.Enums;

namespace TMS.Notifications.Application.UseCases;

public record SendNotificationsCommand() : ICommand<Unit>;

internal sealed class SendNotificationsCommandHandler : IRequestHandler<SendNotificationsCommand, Unit>
{
    private readonly IEmailsService _emails;
    
    private readonly INotificationsRepository _repository;

    private readonly ILogger<SendNotificationsCommandHandler> _logger;

    public SendNotificationsCommandHandler(IEmailsService emails, INotificationsRepository repository, ILogger<SendNotificationsCommandHandler> logger)
    {
        _emails = emails;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(SendNotificationsCommand request, CancellationToken cancellationToken)
    {
        var notifications = await _repository.FindAsync(x => 
            x.Status == NotificationStatus.Pending || x.Status == NotificationStatus.Failed);

        foreach (var notification in notifications)
        {
            try
            {
                await _emails.SendAsync(notification);

                notification.Status = NotificationStatus.Sent;
            }
            catch (Exception e)
            {
                notification.Status = NotificationStatus.Failed;

                _logger.LogError(e, "Failed to send notification");
            }
        }

        foreach (var notification in notifications)
        {
            await _repository.UpdateAsync(notification);
        }

        return Unit.Value;
    }
}