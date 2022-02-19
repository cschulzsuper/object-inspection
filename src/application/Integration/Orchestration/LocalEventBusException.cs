using System;

namespace Super.Paula.Application.Orchestration
{
    public class LocalEventBusException : Exception
    {
        public LocalEventBusException(FormattableString message)
            : base(message.ToString())
        {

        }

        public LocalEventBusException(FormattableString message, Exception innerException)
            : base(message.ToString(), innerException)
        {

        }
    }
}
