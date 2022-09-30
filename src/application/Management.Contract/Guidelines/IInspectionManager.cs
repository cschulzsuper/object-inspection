using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Guidelines;

public interface IInspectionManager
{
    ValueTask<Inspection> GetAsync(string inspection);
    IQueryable<Inspection> GetQueryable();
    IAsyncEnumerable<Inspection> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Inspection>, IQueryable<TResult>> query);
    ValueTask InsertAsync(Inspection inspection);
    ValueTask UpdateAsync(Inspection inspection);
    ValueTask DeleteAsync(Inspection inspection);
}