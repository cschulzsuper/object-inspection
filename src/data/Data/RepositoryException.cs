using System;

namespace Super.Paula.Data
{
    public class RepositoryException : Exception
    {
        public RepositoryException(FormattableString message)
            : base(message.ToString())
        {

        }
    }
}
