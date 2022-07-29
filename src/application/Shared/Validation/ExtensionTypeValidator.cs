namespace Super.Paula.Validation
{
    public static class ExtensionTypeValidator
    {
        public static bool IsValid(object value)
            => ValidValuesValidator.IsValid(
                value,
                "inspector",
                "inspection", 
                "business-object",
                "business-object-inspection", 
                "business-object-inspection-audit-record");
    }
}
