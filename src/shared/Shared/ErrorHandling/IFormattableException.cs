﻿namespace Super.Paula.Shared.ErrorHandling;

public interface IFormattableException
{
    string MessageFormat { get; }

    object?[] MessageArguments { get; }
}