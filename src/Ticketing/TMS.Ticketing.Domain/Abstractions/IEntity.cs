namespace TMS.Ticketing.Domain;

public interface IEntity<out TKey>
{
    TKey Id { get; }
}