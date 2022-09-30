using System;

namespace ChristianSchulz.ObjectInspection.Application.Operation.Exceptions;

public class ExtensionNotFoundException : Exception
{
    public ExtensionNotFoundException(FormattableString message)
        : base(message.ToString())
    {

    }

    public ExtensionNotFoundException(FormattableString message, Exception innerException)
        : base(message.ToString(), innerException)
    {

    }
}