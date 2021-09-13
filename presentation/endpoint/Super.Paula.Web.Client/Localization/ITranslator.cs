namespace Super.Paula.Localization
{
    public interface ITranslator
    {
        string this[string value] { get; }
    }
}
