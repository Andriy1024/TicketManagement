using TMS.Common.IntegrationEvents;

namespace TMS.Payments.Application.MessageBrocker;

public interface IMessageBrocker
{
    Task SendAsync(PaymentStatusUpdated @event);
}
