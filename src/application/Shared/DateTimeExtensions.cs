using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula
{
    public static class DateTimeExtensions
    {
        public static (int day, int milliseconds) ToNumbers(this DateTime dateTime)
        {
            var dateOnly = DateOnly.FromDateTime(dateTime);
            var timeSpan = dateTime.TimeOfDay;

            return (dateOnly.DayNumber, (int)timeSpan.TotalMilliseconds);
        }

        public static DateTime ToDateTime(this (int, int) numbers)
        {
            var dateOnly = DateOnly.FromDayNumber(numbers.Item1);
            var timeSpan = TimeSpan.FromMilliseconds(numbers.Item2);
            var timeOnly = TimeOnly.FromTimeSpan(timeSpan);

            return dateOnly.ToDateTime(timeOnly, DateTimeKind.Utc);
        }

        public static string ToLocalTimeString(this (int, int) numbers)
            => numbers
                .ToDateTime()
                .ToLocalTime()
                .ToString();
    }
}
