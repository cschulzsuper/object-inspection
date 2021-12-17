namespace Super.Paula.Validation
{
    public static class DayNumberValidator
    {
        public static bool IsValid(object dayNumber)
            => dayNumber is >= 0;
    }
}
