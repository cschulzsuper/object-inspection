using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IIdentityManager
    {
        ValueTask<Identity> GetAsync(string identity);
        IQueryable<Identity> GetQueryable();
        IAsyncEnumerable<Identity> GetAsyncEnumerable();
        IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Identity>, IQueryable<TResult>> query);
        ValueTask InsertAsync(Identity identity);
        ValueTask UpdateAsync(Identity identity);
        ValueTask DeleteAsync(Identity identity);
       
    }
}