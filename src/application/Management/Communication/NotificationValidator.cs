using System;
using Super.Paula.Validation;

namespace Super.Paula.Application.Communication
{
    public static class NotificationValidator
    {
        public static (bool, Func<(string, FormattableString)>) DateIsPositive(int date)
            => (DayNumberValidator.IsValid(date),
                () => (nameof(date), $"Date '{date}' must be positive"));

        public static (bool, Func<(string, FormattableString)>) TimeIsInDayTimeRange(int time)
            => (MillisecondsValidator.IsValid(time),
                () => (nameof(time), $"Time '{time}' must be positive and less than 86400000"));

        public static (bool, Func<(string, FormattableString)>) InspectorIsNotEmpty(string inspector)
            => (!string.IsNullOrWhiteSpace(inspector),
                () => (nameof(inspector), $"Inspector can not be empty"));

        public static (bool, Func<(string, FormattableString)>) InspectorHasKebabCase(string inspector)
            => (string.IsNullOrWhiteSpace(inspector) || KebabCaseValidator.IsValid(inspector),
                () => (nameof(inspector), $"Inspector '{inspector}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) InspectorIsNotTooLong(string inspector)
            => (string.IsNullOrWhiteSpace(inspector) || inspector.Length <= 140,
                () => (nameof(inspector), $"Inspector '{inspector}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) InspectorHasValidValue(string inspector)
            => (string.IsNullOrWhiteSpace(inspector) || UniqueNameValidator.IsValid(inspector),
                () => (nameof(inspector), $"Inspector '{inspector}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) TargetIsNotEmpty(string target)
            => (!string.IsNullOrWhiteSpace(target),
                () => (nameof(target), $"Target can not be empty"));

        public static (bool, Func<(string, FormattableString)>) TargetIsRelativeUri(string target)
            => (string.IsNullOrWhiteSpace(target) || Uri.TryCreate(target, UriKind.Relative, out _),
                () => (nameof(target), $"Target '{target}' must be a relative uri"));

        public static (bool, Func<(string, FormattableString)>) TargetIsNotTooLong(string target)
            => (string.IsNullOrWhiteSpace(target) || target.Length <= 140,
                () => (nameof(target), $"Target '{target}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) TextHasValue(string text)
            => (text != null,
                () => (nameof(text), $"Text musst have a value"));

        public static (bool, Func<(string, FormattableString)>) TextIsNotTooLong(string text)
            => (text == null || text.Length <= 140,
                () => (nameof(text), $"Text '{text}' can not have more than 140 characters"));
    }
}
