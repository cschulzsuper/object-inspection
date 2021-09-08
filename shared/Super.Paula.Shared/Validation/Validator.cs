using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Validation
{
    public static class Validator
    {
        public static void Ensure(params (Func<bool,bool> assertion, FormattableString messsage)[] ensureances)
        {
            var valid = true;
            foreach (var (assertion, message) in ensureances)
            {
                valid = assertion.Invoke(valid);

                if (!valid)
                {
                    throw new ValidationException(message);
                }
            }

        }
    }
}
