using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Super.Paula.Validation
{
    public static class Validator
    {
        public static void Ensure(FormattableString term, IEnumerable<(bool, Func<(string, FormattableString)>)> ensureances)
        {
            var errors = new Dictionary<string, ISet<FormattableString>>();
            foreach (var (assertion, errorFunc) in ensureances)
            {
                if (!assertion)
                {
                    var error = errorFunc();

                    var errorItem = errors.GetValueOrDefault(error.Item1, new HashSet<FormattableString>());
                    errorItem.Add(error.Item2);

                    errors[error.Item1] = errorItem;
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(
                    FormattableStringFactory.Create($"Validation of {term.Format} failed", TrimArguments(term.GetArguments())),
                    errors.ToDictionary(
                        x => x.Key,
                        y => y.Value
                            .Select(x => FormattableStringFactory.Create(x.Format, TrimArguments(x.GetArguments())))
                            .ToArray()));
            }
        }

        public static void Ensure(FormattableString term, params (bool, Func<(string, FormattableString)>)[] ensureances)
            => Ensure(term, ensureances.AsEnumerable());

        public static object?[] TrimArguments(object?[] arguments)
            => arguments
                .Select(arg => arg is string stringArg ? stringArg?[..Math.Min(stringArg.Length, 140)] : arg)
                .ToArray();
    }
}
