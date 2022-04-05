namespace Super.Paula.Validation
{
    public static class EventProcessingStateValidator
    {
        public static bool IsValid(object value)
            => ValidValuesValidator.IsValid(value, string.Empty, "in-progress", "completed", "failed");
    }
}
