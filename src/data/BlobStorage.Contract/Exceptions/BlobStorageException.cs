using Super.Paula.Shared.ErrorHandling;
using System;

namespace Super.Paula.BlobStorage.Exceptions;

public class BlobStorageException : Exception, IFormattableException
{
    public string MessageFormat { get; }
    public object?[] MessageArguments { get; }

    public BlobStorageException(FormattableString message)
        : base(message.ToString())
    {
        MessageFormat = message.Format;
        MessageArguments = message.GetArguments();
    }

    public BlobStorageException(FormattableString message, Exception innerException)
        : base(message.ToString(), innerException)
    {
        MessageFormat = message.Format;
        MessageArguments = message.GetArguments();
    }
}