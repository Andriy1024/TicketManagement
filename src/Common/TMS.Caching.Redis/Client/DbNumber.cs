﻿namespace TMS.Caching.Redis;

public abstract record DBNumber(int Value);

public sealed record DB0() : DBNumber(0);

public sealed record DB1() : DBNumber(1);

public sealed record DB2() : DBNumber(2);

public sealed record DB3() : DBNumber(3);

public sealed record DB4() : DBNumber(4);

public sealed record DB5() : DBNumber(5);