namespace TMS.Ticketing.Domain;

public interface IDocumentEntry<TKey>
{
    public abstract static string Collection { get; }

    public TKey Id { get; }
}