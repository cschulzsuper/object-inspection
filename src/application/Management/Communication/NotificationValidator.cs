using System;
using System.Linq;
using Super.Paula.Validation;

namespace Super.Paula.Application.Guidlines
{
    public static class NotificationValidator
    {
        public static (bool, Func<(string, FormattableString)>) DateIsPositive(int date)
            => (date >= 0,
                    () => (nameof(date), $"Date '{date}' must be positive"));

        public static (bool, Func<(string, FormattableString)>) TimeIsInDayTimeRange(int time)
            => (time >= 0 && time < 86400000,
                    () => (nameof(time), $"Time '{time}' must be positive and less than 86400000"));
    }
}
