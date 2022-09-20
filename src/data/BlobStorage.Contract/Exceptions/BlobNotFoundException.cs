using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using System;

namespace ChristianSchulz.ObjectInspection.BlobStorage.Exceptions;

public class BlobNotFoundException : Exception, IFormattableException
{
    public string MessageFormat { get; }
    public object?[] MessageArguments { get; }

    public BlobNotFoundException(FormattableString message)
        : base(message.ToString())
    {
        MessageFormat = message.Format;
        MessageArguments = message.GetArguments();
    }

    public BlobNotFoundException(FormattableString message, Exception innerException)
        : base(message.ToString(), innerException)
    {
        MessageFormat = message.Format;
        MessageArguments = message.GetArguments();
    }
}