namespace Super.Paula.Validation
{
    public static class MillisecondsValidator
    {
        public static bool IsValid(object milliseconds)
            => milliseconds is >= 0 and < 86400000;
    }
}
