using Super.Paula.ErrorHandling;

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
