namespace TMS.Caching.Redis;

public abstract record DBNumber(int Value);

public sealed record DB0() : DBNumber(0);

public sealed record DB1() : DBNumber(1);

public sealed record DB2() : DBNumber(2);

public sealed record DB3() : DBNumber(3);

public sealed record DB4() : DBNumber(4);

public sealed record DB5() : DBNumber(5);

public sealed record DB6() : DBNumber(6);

public sealed record DB7() : DBNumber(7);

public sealed record DB8() : DBNumber(8);

public sealed record DB9() : DBNumber(9);

public sealed record DB10() : DBNumber(10);