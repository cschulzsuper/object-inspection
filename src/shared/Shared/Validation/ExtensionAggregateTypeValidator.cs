using Super.Paula.Application.Operation;

namespace Super.Paula.Shared.Validation;

public static class ExtensionAggregateTypeValidator
{
    public static bool IsValid(object value)
        => ValidValueValidator.IsValid(value, ExtensionAggregateTypes.All);
}