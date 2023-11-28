using TMS.Common.Extensions;
using TMS.Payments.Domain.Abstractions;
using TMS.Payments.Application.Interfaces;

using Marten;

namespace TMS.Payments.Persistence.Implementations;

public sealed class MartenEventStore : IPaymentsEventStore
{
    private readonly IDocumentSession _documentSession;

    public MartenEventStore(IDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }

    public async Task<T?> LoadAsync<T>(string id)
        where T : class, IEventSourcedAggregate, new()
    {
        id.ThrowIfNullOrEmpty();

        var events = await _documentSession.Events.FetchStreamAsync(id);

        if (events.Count == 0) return null;

        var aggregate = new T();

        aggregate.Load(events.Select(e => e.Data).Cast<IDomainEvent>());

        return aggregate;
    }

    public async Task StoreAsync<T>(T aggregate) where T : IEventSourcedAggregate
    {
        var uncommitedChanges = aggregate.Changes;

        _documentSession.Events.Append(aggregate.GetId(), aggregate.Version, uncommitedChanges);

        await _documentSession.SaveChangesAsync();
    }
}