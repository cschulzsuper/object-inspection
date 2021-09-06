using Super.Paula.Shared.ErrorHandling;

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
