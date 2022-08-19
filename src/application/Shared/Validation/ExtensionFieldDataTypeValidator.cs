namespace Super.Paula.Validation
{
    public static class ExtensionFieldDataTypeValidator
    {
        public static bool IsValid(object value)
            => ValidValueValidator.IsValid(
                value,
                "string",
                "boolean", 
                "number");
    }
}
