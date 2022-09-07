namespace Super.Paula.Shared.Validation;

public static class UniqueNameValidator
{
    public static bool IsValid(object value)
    {
        return InvalidValueValidator.IsValid(value,
            string.Empty,
            "business-object",
            "business-object-inspector",
            "business-object-inspection",
            "business-object-inspection-audit",
            "business-object-inspection-audit-record",
            "identity",
            "extension",
            "extension",
            "identity-inspector",
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