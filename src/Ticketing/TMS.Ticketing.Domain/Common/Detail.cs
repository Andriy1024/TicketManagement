﻿namespace TMS.Ticketing.Domain.Common;

public sealed class Detail
{
    public required string Name { get; init; }
    public required string Value { get; init; }
}