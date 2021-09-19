using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Validation
{
    public static class Validator
    {
        [Obsolete]
        public static void Ensure(IEnumerable<(Func<bool, bool> assertion, Func<FormattableString> messsage)> ensureances)
        {
            var valid = true;
            foreach (var (assertion, message) in ensureances)
            {
                valid = assertion.Invoke(valid);

                if (!valid)
                {
                    throw new ValidationException(message());
                }
            }

        }

        [Obsolete]
        public static void Ensure(params (Func<bool, bool> assertion, Func<FormattableString> messsage)[] ensureances)
            => Ensure(ensureances.AsEnumerable());

        public static void Ensure(IEnumerable<(bool, Func<(string,FormattableString)>)> ensureances)
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
                    $"A validation error occured", 
                    errors.ToDictionary(
                        x => x.Key, 
                        y =>y.Value.ToArray()));
            }
        }

        public static void Ensure(params (bool, Func<(string, FormattableString)>)[] ensureances)
            => Ensure(ensureances.AsEnumerable());
    }
}
