using System;
using System.Linq;
using Super.Paula.Application.Communication;
using Super.Paula.Validation;

namespace Super.Paula.Application.Guidlines
{
    public static class NotificationValidator
    {
        public static (bool, Func<(string, FormattableString)>) DateIsPositive(int date)
            => (DayNumberValidator.IsValid(date),
                    () => (nameof(Notification.Date), $"Date '{date}' must be positive"));

        public static (bool, Func<(string, FormattableString)>) TimeIsInDayTimeRange(int time)
            => (MillisecondsValidator.IsValid(time),
                    () => (nameof(Notification.Time), $"Time '{time}' must be positive and less than 86400000"));

        public static (bool, Func<(string, FormattableString)>) InspectorIsNotNull(string inspector)
            => (inspector != null,
                    () => (nameof(Notification.Inspector), $"Inspector can not be null"));

        public static (bool, Func<(string, FormattableString)>) InspectorHasKebabCase(string inspector)
            => (KebabCaseValidator.IsValid(inspector),
                    () => (nameof(Notification.Inspector), $"Inspector '{inspector}' must be in kebab case"));

        public static (bool, Func<(string, FormattableString)>) TargetIsNotNull(string target)
            => (!string.IsNullOrWhiteSpace(target),
                    () => (nameof(Notification.Target), $"Target can not be null"));

        public static (bool, Func<(string, FormattableString)>) TargetIsRelativeUri(string target)
            => (Uri.TryCreate(target,UriKind.Relative,out _),
                    () => (nameof(Notification.Target), $"Target must be a relative uri"));

        public static (bool, Func<(string, FormattableString)>) TextIsNotNull(string text)
            => (!string.IsNullOrWhiteSpace(text),
                    () => (nameof(Notification.Text), $"Text can not be null"));

        public static (bool, Func<(string, FormattableString)>) TextIsNotTooLong(string text)
            => (text == null || text.Length <= 140,
                    () => (nameof(Notification.Text), $"Text can not have more than 140 characters"));
    }
}
