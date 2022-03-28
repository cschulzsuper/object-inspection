namespace Super.Paula.Validation
{
    public static class WorkerStateValidator
    {
        public static bool IsValid(object value)
            => ValidValuesValidator.IsValid(value, string.Empty, "starting", "running", "finished", "failed");
    }
}
