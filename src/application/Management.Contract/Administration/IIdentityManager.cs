using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IIdentityManager
    {
        ValueTask<Identity> GetAsync(string identity);
        ValueTask InsertAsync(Identity identity);
        ValueTask UpdateAsync(Identity identity);
        ValueTask DeleteAsync(Identity identity);
    }
}