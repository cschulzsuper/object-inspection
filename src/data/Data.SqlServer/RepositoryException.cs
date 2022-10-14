using System;

namespace ChristianSchulz.ObjectInspection.Data;

public class RepositoryException : Exception
{
    public RepositoryException(FormattableString message)
        : base(message.ToString())
    {

    }
}