namespace Super.Paula.Shared.Validation;

public static class AuditResultValidator
{
    public static bool IsValid(object value)
        => ValidValueValidator.IsValid(value, string.Empty, "satisfying", "insufficient", "failed");
}