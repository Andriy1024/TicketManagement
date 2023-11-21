using MediatR;

namespace TMS.Common.IntegrationEvents;

public class IntegrationEvent<TPayload> : IRequest<Unit>
{
    public TPayload Payload { get; set; }
}