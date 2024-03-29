﻿using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;

namespace ChristianSchulz.ObjectInspection.Application.Orchestration;

public static class EventValidator
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

    public static (bool, Func<(string, FormattableString)>) StateIsNotNull(string state)
        => (state != null,
            () => (nameof(state), $"State can not be null"));

    public static (bool, Func<(string, FormattableString)>) StateHasValidValue(string state)
        => (state == null || EventStateValidator.IsValid(state),
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