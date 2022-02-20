namespace Super.Paula.Validation
{
    public static class UniqueNameValidator
    {
        public static bool IsValid(object value)
        {
            if (value is string stringValue)
            {
                value = stringValue
                    .Replace("-", string.Empty)
                    .Trim();
            }           
            
            return InvalidValuesValidator.IsValid(value, 
                string.Empty,
                "businessobject",
                "businessobjectinspectionaudit",
                "identity",
                "identityinspector",
                "inspection",
                "inspector",
                "organiztation",
                "null",
                "me", 
                "search");
        }
    }
}
