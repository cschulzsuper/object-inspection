using ChristianSchulz.ObjectInspection.Application.Operation;

namespace ChristianSchulz.ObjectInspection.Shared.Validation;

public static class ExtensionAggregateTypeValidator
{
    public static bool IsValid(object value)
        => ValidValueValidator.IsValid(value, ExtensionAggregateTypes.All);
}