namespace Super.Paula.Web.Shared.Localization
{
    public interface ITranslator
    {
        string this[string value] { get; }
    }
}
