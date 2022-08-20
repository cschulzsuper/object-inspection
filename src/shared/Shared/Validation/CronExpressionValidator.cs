using Cronos;
using System;

namespace Super.Paula.Shared.Validation;

public static class CronExpressionValidator
{
    public static bool IsValid(object value)
    {
        if (value is not string cronExpression)
        {
            return false;
        }
        try
        {
            CronExpression.Parse(cronExpression, CronFormat.Standard);
        }
        catch (FormatException)
        {
            return false;
        }

        return true;
    }
}