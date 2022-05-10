namespace Super.Paula.Validation
{
    public interface IFormattableException
    {
        string MessageFormat { get; }

        object?[] MessageArguments { get; }
    }
}