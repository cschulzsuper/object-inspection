namespace Super.Paula.Validation
{
    public static class ExtensionFieldTypeValidator
    {
        public static bool IsValid(object value)
            => ValidValuesValidator.IsValid(
                value,
                "string",
                "boolean", 
                "number");
    }
}
