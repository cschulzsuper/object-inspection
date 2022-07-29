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
                "businessobjectinspection",
                "businessobjectinspectionaudit",
                "businessobjectinspectionauditrecord",
                "identity",
                "extension",
                "identityinspector",
                "inspection",
                "inspector",
                "organization",
                "reset",
                "register",
                "null",
                "me",
                "current",
                "search");
        }
    }
}
