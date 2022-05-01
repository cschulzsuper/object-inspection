namespace Super.Paula.Client.Localization
{
    public record TranslationInfo(string Value, bool Placeholder)
    {
        public TranslationInfo(string Value) : this(Value, false)
        {

        }

        public static implicit operator TranslationInfo(string value)
            => new(value);
    }
}
