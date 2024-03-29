﻿using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using System;

namespace ChristianSchulz.ObjectInspection.Application;

public class ManagementException : Exception, IFormattableException
{
    public string MessageFormat { get; }
    public object?[] MessageArguments { get; }

    public ManagementException(FormattableString message)
        : base(message.ToString())
    {
        MessageFormat = message.Format;
        MessageArguments = message.GetArguments();
    }

    public ManagementException(FormattableString message, Exception innerException)
        : base(message.ToString(), innerException)
    {
        MessageFormat = message.Format;
        MessageArguments = message.GetArguments();
    }
}