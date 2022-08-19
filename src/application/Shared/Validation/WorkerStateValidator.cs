namespace Super.Paula.Validation
{
    public static class WorkerStateValidator
    {
        public static bool IsValid(object value)
            => ValidValueValidator.IsValid(value, string.Empty, "starting", "running", "finished", "failed");
    }
}
