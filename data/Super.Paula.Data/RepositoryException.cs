using Super.Paula.ErrorHandling;

namespace Super.Paula
{
    public class RepositoryException : ErrorException
    {
        public RepositoryException(FormattableString message)
            : base(message)
        {

        }
    }
}
