using Super.Paula.Application.Operation;

namespace Super.Paula.Validation
{
    public static class ExtensionAggregateTypeValidator
    {
        public static bool IsValid(object value)
            => ValidValueValidator.IsValid(value, ExtensionAggregateTypes.All);
    }
}
