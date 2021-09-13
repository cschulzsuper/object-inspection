
namespace Super.Paula.ErrorHandling
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ErrorMessageAttribute : Attribute
    {

        public ErrorMessageAttribute(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}