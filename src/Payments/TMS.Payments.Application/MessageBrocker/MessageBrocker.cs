using TMS.Common.IntegrationEvents;
using TMS.Payments.Application.Ticketing;

namespace TMS.Payments.Application.MessageBrocker;

public sealed class MessageBrocker : IMessageBrocker
{
    private readonly ITicketingApi _ticketingApi;

    public MessageBrocker(ITicketingApi ticketingApi)
    {
        _ticketingApi = ticketingApi;
    }

    public Task SendAsync(PaymentStatusUpdated @event)
    {
        // This webhook will be reimplemented as message brocker message in the message queues module
        return _ticketingApi.PaymentStatusUpdatedAsync(new IntegrationEvent<PaymentStatusUpdated>
        {
            Payload = @event
        });
    }
}
