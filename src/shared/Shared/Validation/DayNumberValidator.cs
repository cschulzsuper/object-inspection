namespace Super.Paula.Shared.Validation;

public static class DayNumberValidator
{
    public static bool IsValid(object dayNumber)
        => dayNumber is >= 0;
}