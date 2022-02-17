namespace Super.Paula.Validation
{
    public static class UniqueNameValidator
    {
        public static bool IsValid(object value)
            => InvalidValuesValidator.IsValid(value, string.Empty, "me", "search");
    }
}
