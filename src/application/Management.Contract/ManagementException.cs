using System;

namespace Super.Paula.Application
{
    public class ManagementException : Exception
    {
        public ManagementException(FormattableString message)
            : base(message.ToString())
        {

        }

        public ManagementException(FormattableString message, Exception innerException)
            : base(message.ToString(), innerException)
        {

        }
    }
}
