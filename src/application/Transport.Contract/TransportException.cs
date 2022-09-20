using System;

namespace ChristianSchulz.ObjectInspection.Application;

public class TransportException : Exception
{
    public TransportException(FormattableString message)
        : base(message.ToString())
    {

    }

    public TransportException(FormattableString message, Exception innerException)
        : base(message.ToString(), innerException)
    {

    }
}