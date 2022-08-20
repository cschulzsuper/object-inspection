using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration;

public interface IWorkerManager
{
    ValueTask<Worker> GetAsync(string worker);
    IQueryable<Worker> GetQueryable();
    IAsyncEnumerable<Worker> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Worker>, IQueryable<TResult>> query);
    ValueTask InsertAsync(Worker worker);
    ValueTask UpdateAsync(Worker worker);
    ValueTask DeleteAsync(Worker worker);
}