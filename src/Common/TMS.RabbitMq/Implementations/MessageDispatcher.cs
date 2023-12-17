using MediatR;

using Microsoft.Extensions.DependencyInjection;

using TMS.RabbitMq.Pipeline;

namespace TMS.RabbitMq.Implementations;

internal class MessageDispatcher : IMessageDispatcher
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MessageDispatcher(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task HandleAsync(SubscriberRequest input)
    {
        await using (var scope = _scopeFactory.CreateAsyncScope()) 
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(input.IntegrationEvent);
        }
    }
}