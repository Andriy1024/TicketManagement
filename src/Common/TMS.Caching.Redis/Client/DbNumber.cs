namespace TMS.Caching.Redis;

public abstract record DbNumber(int Value);

public sealed record DB0() : DbNumber(0);

public sealed record DB1() : DbNumber(1);

public sealed record DB2() : DbNumber(2);

public sealed record DB3() : DbNumber(3);

public sealed record DB4() : DbNumber(4);

public sealed record DB5() : DbNumber(5);