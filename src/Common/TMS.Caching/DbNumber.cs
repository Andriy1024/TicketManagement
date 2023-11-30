namespace TMS.Caching;

public abstract class DbNumber
{
    public readonly int Value;
    public DbNumber(int value) => Value = value;
}

public sealed class DB0 : DbNumber
{
    public DB0() : base(0) { }
}

public sealed class DB1 : DbNumber
{
    public DB1() : base(1) { }
}