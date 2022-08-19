namespace Super.Paula.Validation
{
    public static class EventStateValidator
    {
        public static bool IsValid(object value)
            => ValidValueValidator.IsValid(value, string.Empty, "in-progress", "completed", "failed");
    }
}
