using System;

namespace ChristianSchulz.ObjectInspection.Application.Operation.Exceptions;

public class DistinctionTypeNotFoundException : Exception
{
    public DistinctionTypeNotFoundException(FormattableString message)
        : base(message.ToString())
    {

    }

    public DistinctionTypeNotFoundException(FormattableString message, Exception innerException)
        : base(message.ToString(), innerException)
    {

    }
}