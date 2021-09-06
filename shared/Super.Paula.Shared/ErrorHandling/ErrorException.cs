namespace Super.Paula.Shared.ErrorHandling
{
    public class ErrorException : Exception
    {
        public ErrorException(FormattableString message)
            : base(message.ToString())
        {

        }
    }
}