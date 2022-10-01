using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IDistinctionTypeManager
{
    DistinctionType Get(string distinctionType);
    ValueTask<DistinctionType> GetAsync(string distinctionType);
    IQueryable<DistinctionType> GetQueryable();

    IAsyncEnumerable<DistinctionType> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<DistinctionType>, IQueryable<TResult>> query);

    ValueTask InsertAsync(DistinctionType distinctionType);
    ValueTask UpdateAsync(DistinctionType distinctionType);
    ValueTask DeleteAsync(DistinctionType distinctionType);
}