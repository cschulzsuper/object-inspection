namespace Super.Paula.ErrorHandling
{
    public interface IFormattableException
    {
        string MessageFormat { get; }

        object?[] MessageArguments { get; }
    }
}