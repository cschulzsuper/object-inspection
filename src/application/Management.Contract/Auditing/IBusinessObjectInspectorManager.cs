using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectorManager
{
    ValueTask<BusinessObjectInspector> GetAsync(string businessObject, string inspector);
    IQueryable<BusinessObjectInspector> GetQueryable();

    IAsyncEnumerable<BusinessObjectInspector> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObjectInspector>, IQueryable<TResult>> query);

    ValueTask InsertAsync(BusinessObjectInspector inspector);
    ValueTask UpdateAsync(BusinessObjectInspector inspector);
    ValueTask DeleteAsync(BusinessObjectInspector inspector);
}