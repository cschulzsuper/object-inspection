using ChristianSchulz.ObjectInspection.Shared;

namespace ChristianSchulz.ObjectInspection.Client.Localization;

public class TranslationCategoryProvider
{
    public string? Get<T>()
    {
        var typeName = typeof(T).FullName;
        var category = ConvertOrDefault(typeName);

        return category;
    }

    private string? ConvertOrDefault(string? typeName)
    {
        if (typeName == null)
        {
            return null;
        }

        var typeNameWithoutRoot = typeName.Replace("ChristianSchulz.ObjectInspection.Client", string.Empty);

        var categoryCamelCase = typeNameWithoutRoot
            .Replace(".", string.Empty)
            .Replace("_", string.Empty);
        var categoryKebabCase = CaseStyleConverter.FromPascalCaseToKebabCase(categoryCamelCase);

        return categoryKebabCase;
    }
}