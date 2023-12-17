using TMS.Common.IntegrationEvents;
using TMS.Payments.Application.Interfaces;
using TMS.Payments.Infrastructure.Ticketing;
using TMS.RabbitMq;

namespace TMS.Payments.Infrastructure.MessageBrocker;

public sealed class PaymentsMessageBrocker : IPaymentsMessageBrocker
{
    private readonly ITicketingApi _ticketingApi;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public PaymentsMessageBrocker(ITicketingApi ticketingApi, IRabbitMqPublisher rabbitMqPublisher)
    {
        _ticketingApi = ticketingApi;
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    public Task SendAsync(PaymentStatusUpdated @event)
    {
        //// This webhook will be reimplemented as message brocker message in the message queues module
        //return _ticketingApi.PaymentStatusUpdatedAsync(new IntegrationEvent<PaymentStatusUpdated>
        //{
        //    Payload = @event
        //});

        _rabbitMqPublisher.Publish(new IntegrationEvent<PaymentStatusUpdated>
        {
            Payload = @event
        }, 
        p =>
        {
            p.Exchange.Name = Exchange.Name.Payments;
            p.Exchange.Type = Exchange.Type.Direct;
            p.EnableRetryPolicy = true;
        });

        return Task.CompletedTask;
    }
}
