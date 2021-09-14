using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Validation
{
    public static class Validator
    {
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

        public static void Ensure(params (Func<bool, bool> assertion, Func<FormattableString> messsage)[] ensureances)
            => Ensure(ensureances.AsEnumerable());
    }
}
