using System;
using System.Runtime.CompilerServices;

namespace Super.Paula
{
    public static class TypeNameConverterNew
    {
        public static string ToKebabCase(Type type)
        {
            var typeName = type.Name;
            var estimateLength = EstimateLength(typeName);
            var resultSpan = (Span<char>) stackalloc char[estimateLength];
            
            var j = 0;
            for (var i = 0; i < typeName.Length; i++)
            {
                // if current char is the first char
                if (i == 0)
                {
                    resultSpan[j++] = char.ToLower(typeName[i]);
                    continue;
                }

                // if current char is the first char
                if (i == 0)
                {
                    resultSpan[j++] = char.ToLower(typeName[i]);
                    continue;
                }

                // if current char is already lowercase
                if (char.IsLower(typeName[i]))
                {
                    resultSpan[j++] = typeName[i];
                    continue;
                }

                // if current char is a number and the previous is not
                if (char.IsDigit(typeName[i]))
                {
                    throw new NotImplementedException("Currently, types with digits are not supported");
                }

                // if current char is upper and next is upper
                if (char.IsUpper(typeName[i + 1]))
                {
                    throw new NotImplementedException("Currently, types with two consecutive uppercase characters are not supported.");
                }

                // if current char is upper and previous char is lower
                resultSpan[j++] = '-';
                resultSpan[j++] = char.ToLower(typeName[i]);
                continue;
            }

            return resultSpan.Slice(0, j).ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int EstimateLength(string typeName)
        {
            var estimatedLength = typeName.Length;

            for (int i = 0; i < typeName.Length; i++)
            {
                if (char.IsUpper(typeName[i]))
                {
                    estimatedLength++;
                }
            }

            return estimatedLength;
        }
    }
}
