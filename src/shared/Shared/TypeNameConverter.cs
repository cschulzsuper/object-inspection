using System;
using System.Collections.Concurrent;

namespace Super.Paula.Shared;

public static class TypeNameConverter
{
    private static readonly ConcurrentDictionary<Type, string> _typeNameCache = new ConcurrentDictionary<Type, string>
    {
        [typeof(char)] = "char",
        [typeof(decimal)] = "decimal",
        [typeof(byte)] = "byte",
        [typeof(sbyte)] = "sbyte",
        [typeof(short)] = "short",
        [typeof(ushort)] = "ushort",
        [typeof(int)] = "int",
        [typeof(uint)] = "uint",
        [typeof(long)] = "long",
        [typeof(ulong)] = "ulong",
        [typeof(Int128)] = "int128",
        [typeof(UInt128)] = "uint128",
        [typeof(nint)] = "nint",
        [typeof(nuint)] = "nuint",
        [typeof(Half)] = "half",
        [typeof(float)] = "float",
        [typeof(double)] = "double",
        [typeof(object)] = "object",
        [typeof(string)] = "string",
    };

    public static string ToKebabCase(Type type)
    {
        var kebabCase = _typeNameCache.GetOrAdd(type, type =>
            {
                var pascalCase = type.Name;
                return CaseStyleConverter.FromPascalCaseToKebabCase(pascalCase);
            });

        return kebabCase;
    }
}