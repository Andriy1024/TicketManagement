namespace TMS.Ticketing.Domain;

public interface ICollectionEntry<TKey>
{
    abstract static string Collection { get; }

    TKey Id { get; }
}