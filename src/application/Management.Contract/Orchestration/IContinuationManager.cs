using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration;

public interface IContinuationManager
{
    ValueTask<Continuation> GetAsync(string id);
    IQueryable<Continuation> GetQueryable();
    IAsyncEnumerable<Continuation> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Continuation>, IQueryable<TResult>> query);
    ValueTask InsertAsync(Continuation continuation);
    ValueTask UpdateAsync(Continuation continuation);
    ValueTask DeleteAsync(Continuation continuation);
}