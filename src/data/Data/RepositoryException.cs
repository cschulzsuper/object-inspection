using Super.Paula.ErrorHandling;
using System;

namespace Super.Paula.Data
{
    public class RepositoryException : ErrorException
    {
        public RepositoryException(FormattableString message)
            : base(message)
        {

        }
    }
}
