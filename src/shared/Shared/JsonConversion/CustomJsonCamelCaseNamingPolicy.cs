using System;
using System.Text.Json;

namespace Super.Paula.Shared.JsonConversion;

public class CustomJsonCamelCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
        {
            return name;
        }

        if (string.Equals(name, "etag", StringComparison.InvariantCultureIgnoreCase))
        {
            return name.ToLower();
        }

        if (string.Equals(name, "btag", StringComparison.InvariantCultureIgnoreCase))
        {
            return name.ToLower();
        }

#if NETCOREAPP
        return string.Create(name.Length, name, (chars, name) =>
        {
            name
#if !NETCOREAPP
        .AsSpan()
#endif
            .CopyTo(chars);
            FixCasing(chars);
        });
#else
    char[] chars = name.ToCharArray();
    FixCasing(chars);
    return new string(chars);
#endif
    }

    private static void FixCasing(Span<char> chars)
    {
        for (int i = 0; i < chars.Length; i++)
        {
            if (i == 1 && !char.IsUpper(chars[i]))
            {
                break;
            }

            bool hasNext = i + 1 < chars.Length;

            // Stop when next char is already lowercase.
            if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
            {
                // If the next char is a space, lowercase current char before exiting.
                if (chars[i + 1] == ' ')
                {
                    chars[i] = char.ToLowerInvariant(chars[i]);
                }

                break;
            }

            chars[i] = char.ToLowerInvariant(chars[i]);
        }
    }
}