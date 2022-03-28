namespace Super.Paula.Validation
{
    public static class ContinuationStateValidator
    {
        public static bool IsValid(object value)
            => ValidValuesValidator.IsValid(value, string.Empty, "in-progress", "completed", "failed");
    }
}
