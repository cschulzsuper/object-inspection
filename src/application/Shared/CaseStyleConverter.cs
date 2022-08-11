using System.Text;

namespace Super.Paula
{
    public static class CaseStyleConverter
    {
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
    }
}
