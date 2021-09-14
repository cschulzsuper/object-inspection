namespace Super.Paula.ErrorHandling
{
    public class ErrorException : Exception
    {
        public ErrorException(FormattableString message)
            : base(message.ToString())
        {

        }
    }
}