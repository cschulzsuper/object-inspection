namespace ChristianSchulz.ObjectInspection.Shared.Validation;

public static class ExtensionFieldDataNameValidator
{
    public static bool IsValid(object value)
        => InvalidValueValidator.IsValid(
            value,
            "etag",
            "displayName",
            "uniqueName",
            "inspector");
}