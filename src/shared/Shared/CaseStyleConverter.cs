using System;
using System.Text;

namespace Super.Paula.Shared;

public static class CaseStyleConverter
{
    public static string FromCamelCaseToKebabCase(string camelCase)
        => FromPascalCaseToKebabCase(camelCase);

    public static string FromPascalCaseToKebabCase(string pascalCase)
    {
        var kebabCase = new StringBuilder();

        for (var i = 0; i < pascalCase.Length; i++)
        {
            // if current char is already lowercase
            if (char.IsLower(pascalCase[i]))
            {
                kebabCase.Append(pascalCase[i]);
                continue;
            }

            // if current char is the first char
            if (i == 0)
            {
                kebabCase.Append(char.ToLower(pascalCase[i]));
                continue;
            }

            // if current char is a number and the previous is not number and not upper
            if (char.IsDigit(pascalCase[i]) && !char.IsDigit(pascalCase[i - 1]) && !char.IsUpper(pascalCase[i - 1]))
            {
                kebabCase.Append('-');
                kebabCase.Append(pascalCase[i]);
                continue;
            }

            // if current char is a number and previous is
            if (char.IsDigit(pascalCase[i]))
            {
                kebabCase.Append(pascalCase[i]);
                continue;
            }

            // if current char is upper and previous char is lower
            if (char.IsLower(pascalCase[i - 1]))
            {
                kebabCase.Append('-');
                kebabCase.Append(char.ToLower(pascalCase[i]));
                continue;
            }

            // if current char is upper and next char doesn't exist or is upper or is a number
            if (i + 1 == pascalCase.Length || char.IsUpper(pascalCase[i + 1]) || char.IsDigit(pascalCase[i + 1]))
            {
                kebabCase.Append(char.ToLower(pascalCase[i]));
                continue;
            }

            kebabCase.Append('-');
            kebabCase.Append(char.ToLower(pascalCase[i]));
        }

        return kebabCase.ToString();
    }

    public static string FromPascalCaseToCamelCase(string pascalCase)
    {
        if (string.Equals(pascalCase, "etag", StringComparison.InvariantCultureIgnoreCase))
        {
            return pascalCase.ToLower();
        }

        if (string.Equals(pascalCase, "btag", StringComparison.InvariantCultureIgnoreCase))
        {
            return pascalCase.ToLower();
        }

        if (string.IsNullOrEmpty(pascalCase) || !char.IsUpper(pascalCase[0]))
        {
            return pascalCase;
        }

        var camelCase = string.Create(pascalCase.Length, pascalCase, 
            (chars, result) =>
                {
                    result.CopyTo(chars);
                    InternalFromPascalCaseToCamelCase(chars);
                });

        return camelCase;
    }

    private static void InternalFromPascalCaseToCamelCase(Span<char> chars)
    {
        for (var i = 0; i < chars.Length; i++)
        {
            if (i == 1 && !char.IsUpper(chars[i]))
            {
                break;
            }

            var hasNext = i + 1 < chars.Length;

            // Stop when next char is already lowercase.
            if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
            {
                break;
            }

            chars[i] = char.ToLowerInvariant(chars[i]);
        }
    }
}