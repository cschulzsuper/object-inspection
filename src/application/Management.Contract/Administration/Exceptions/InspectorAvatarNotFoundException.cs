using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using System;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Exceptions;

public class InspectorAvatarStreamInvalidException : Exception, IFormattableException
{
    public string MessageFormat { get; }
    public object?[] MessageArguments { get; }

    public InspectorAvatarStreamInvalidException(FormattableString message)
        : base(message.ToString())
    {
        MessageFormat = message.Format;
        MessageArguments = message.GetArguments();
    }

    public InspectorAvatarStreamInvalidException(FormattableString message, Exception innerException)
        : base(message.ToString(), innerException)
    {
        MessageFormat = message.Format;
        MessageArguments = message.GetArguments();
    }
}