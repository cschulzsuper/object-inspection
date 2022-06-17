using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Super.Paula
{
    public static class TypeNameConverter
    {
        public static string ToKebabCase(Type type)
        {
            var pascalCase = type.Name;
            var kebabCase = new StringBuilder();

            for (var i = 0; i < pascalCase.Length; i++)
            {
                // if current char is the first char
                if (i == 0)
                {
                    kebabCase.Append(char.ToLower(pascalCase[i]));
                    continue;
                }

                // if current char is already lowercase
                if (char.IsLower(pascalCase[i])) 
                {
                    kebabCase.Append(pascalCase[i]);
                    continue;
                }

                // if current char is a number and the previous is not
                if (char.IsDigit(pascalCase[i]))
                {
                    throw new NotImplementedException("Types with digits are not supported");
                }

                // if current char is upper and next is upper
                if (char.IsUpper(pascalCase[i + 1]))
                {
                    throw new NotImplementedException("Types with two consecutive uppercase characters are not supported");
                }

                // if current char is upper
                kebabCase.Append('-');
                kebabCase.Append(char.ToLower(pascalCase[i]));
                continue;
            }

            return kebabCase.ToString();
        }
    }
}
