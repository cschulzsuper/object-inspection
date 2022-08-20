using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectionManager
{
    ValueTask<BusinessObjectInspection> GetAsync(string businessObject, string inspection);
    IQueryable<BusinessObjectInspection> GetQueryable();

    IAsyncEnumerable<BusinessObjectInspection> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObjectInspection>, IQueryable<TResult>> query);

    ValueTask InsertAsync(BusinessObjectInspection inspection);
    ValueTask UpdateAsync(BusinessObjectInspection inspection);
    ValueTask DeleteAsync(BusinessObjectInspection inspection);
}