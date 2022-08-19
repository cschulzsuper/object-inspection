using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Operation
{
    public static class ExtensionFieldDataTypes
    {
        private readonly static IDictionary<string, Type> _all = new Dictionary<string, Type>
        {
            [String] = typeof(string),
            [Bool] = typeof(bool)
        };

        public static Type GetClrType(string type)
            => _all[type];

        public readonly static string[] All
            = _all.Keys.ToArray();

        public const string String
            = "string";

        public const string Bool
            = "bool";

    }
}
