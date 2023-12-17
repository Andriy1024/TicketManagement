using TMS.Common.IntegrationEvents;

namespace TMS.Payments.Application.Interfaces;

public interface IPaymentsMessageBrocker
{
    Task SendAsync(PaymentStatusUpdated @event);
}