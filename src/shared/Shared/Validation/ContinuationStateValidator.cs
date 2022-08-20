namespace Super.Paula.Shared.Validation;

public static class ContinuationStateValidator
{
    public static bool IsValid(object value)
        => ValidValueValidator.IsValid(value, string.Empty, "in-progress", "completed", "failed");
}