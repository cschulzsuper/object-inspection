﻿using Super.Paula.ErrorHandling;
using System;

namespace Super.Paula.Application.Administration.Exceptions
{
    public class InspectorAvatarNotFoundException : Exception, IFormattableException
    {
        public string MessageFormat { get; }
        public object?[] MessageArguments { get; }

        public InspectorAvatarNotFoundException(FormattableString message)
            : base(message.ToString())
        {
            MessageFormat = message.Format;
            MessageArguments = message.GetArguments();
        }

        public InspectorAvatarNotFoundException(FormattableString message, Exception innerException)
            : base(message.ToString(), innerException)
        {
            MessageFormat = message.Format;
            MessageArguments = message.GetArguments();
        }
    }
}