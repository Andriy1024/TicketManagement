using TMS.Payments.Domain.Abstractions;

namespace TMS.Payments.Application.Interfaces;

public interface IPaymentsEventStore
{
    /// <summary>
    /// Store uncommited events.
    /// </summary>
    Task StoreAsync<T>(T aggregate) where T : IEventSourcedAggregate;

    /// <summary>
    /// Perform a live aggregation of the raw events in this stream to a T object.
    /// </summary>
    Task<T?> LoadAsync<T>(string id) where T : class, IEventSourcedAggregate, new();
}