using Super.Paula.Validation;
using System;

namespace Super.Paula.Application.Orchestration
{
    public static class EventProcessingValidator
    {
        public static (bool, Func<(string, FormattableString)>) IdIsNotEmpty(string id)
            => (!string.IsNullOrWhiteSpace(id),
                () => (nameof(id), $"Id can not be empty"));

        public static (bool, Func<(string, FormattableString)>) IdIsGuid(string id)
            => (Guid.TryParse(id, out _),
                () => (nameof(id), $"Id must be a {nameof(Guid)}"));

        public static (bool, Func<(string, FormattableString)>) NameIsNotEmpty(string name)
            => (!string.IsNullOrWhiteSpace(name),
                () => (nameof(name), $"Name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) NameHasKebabCase(string name)
            => (string.IsNullOrWhiteSpace(name) || KebabCaseValidator.IsValid(name),
                () => (nameof(name), $"Name '{name}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) NameIsNotTooLong(string name)
            => (string.IsNullOrWhiteSpace(name) || name.Length <= 140,
                () => (nameof(name), $"Name '{name}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) NameHasValidValue(string name)
            => (string.IsNullOrWhiteSpace(name) || UniqueNameValidator.IsValid(name),
                () => (nameof(name), $"Name '{name}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) SubscriberIsNotEmpty(string subscriber)
            => (!string.IsNullOrWhiteSpace(subscriber),
                () => (nameof(subscriber), $"Subscriber can not be empty"));

        public static (bool, Func<(string, FormattableString)>) SubscriberHasKebabCase(string subscriber)
            => (string.IsNullOrWhiteSpace(subscriber) || KebabCaseValidator.IsValid(subscriber),
                () => (nameof(subscriber), $"Subscriber '{subscriber}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) SubscriberIsNotTooLong(string subscriber)
            => (string.IsNullOrWhiteSpace(subscriber) || subscriber.Length <= 140,
                () => (nameof(subscriber), $"Subscriber '{subscriber}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) SubscriberHasValidValue(string subscriber)
            => (string.IsNullOrWhiteSpace(subscriber) || UniqueNameValidator.IsValid(subscriber),
                () => (nameof(subscriber), $"Subscriber '{subscriber}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) StateIsNotNull(string state)
            => (state != null,
                () => (nameof(state), $"State can not be null"));

        public static (bool, Func<(string, FormattableString)>) StateHasValidValue(string state)
            => (state == null || EventProcessingStateValidator.IsValid(state),
                () => (nameof(state), $"State '{state}' is not a valid value"));

        public static (bool, Func<(string, FormattableString)>) OperationIdIsNotEmpty(string operationId)
            => (!string.IsNullOrWhiteSpace(operationId),
                () => (nameof(operationId), $"Operation id can not be empty"));

        public static (bool, Func<(string, FormattableString)>) DataIsNotNull(string data)
            => (data != null,
                () => (nameof(data), $"Data can not be null"));

        public static (bool, Func<(string, FormattableString)>) DataIsNotTooLong(string data)
            => (data == null || data.Length <= 4000,
                () => (nameof(data), $"Data '{data}' can not have more than 4000 characters"));

        public static (bool, Func<(string, FormattableString)>) UserIsNotNull(string user)
            => (user != null,
                () => (nameof(user), $"User can not be null"));

        public static (bool, Func<(string, FormattableString)>) UserIsNotTooLong(string user)
            => (user == null || user.Length <= 4000,
                () => (nameof(user), $"User '{user}' can not have more than 4000 characters"));

        public static (bool, Func<(string, FormattableString)>) CreationDateIsPositive(int creationDate)
            => (DayNumberValidator.IsValid(creationDate),
                () => (nameof(creationDate), $"Creation date '{creationDate}' must be positive"));

        public static (bool, Func<(string, FormattableString)>) CreationTimeIsInDayTimeRange(int creationTime)
            => (MillisecondsValidator.IsValid(creationTime),
                () => (nameof(creationTime), $"Creation time '{creationTime}' must be positive and less than 86400000"));
    }
}
