﻿using System;

namespace Super.Paula.Application.Administration.Exceptions
{
    public class SignOutException : Exception
    {
        public SignOutException(FormattableString message)
            : base(message.ToString())
        {

        }

        public SignOutException(FormattableString message, Exception innerException)
            : base(message.ToString(), innerException)
        {

        }
    }
}
