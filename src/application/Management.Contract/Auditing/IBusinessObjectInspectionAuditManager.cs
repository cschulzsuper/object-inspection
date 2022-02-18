using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionAuditManager
    {
        ValueTask<BusinessObjectInspectionAudit> GetAsync(string businessObject, string inspection, int date, int time);

        ValueTask<BusinessObjectInspectionAudit?> GetOrDefaultAsync(string businessObject, string inspection, int date, int time);

        IQueryable<BusinessObjectInspectionAudit> GetQueryable();

        IAsyncEnumerable<BusinessObjectInspectionAudit> GetAsyncEnumerable(
            CancellationToken cancellationToken = default);

        IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObjectInspectionAudit>, IQueryable<TResult>> query, 
            CancellationToken cancellationToken = default);

        IQueryable<BusinessObjectInspectionAudit> GetDateBasedQueryable(int date);
        IAsyncEnumerable<BusinessObjectInspectionAudit> GetDateBasedAsyncEnumerable(int date);
        IAsyncEnumerable<TResult> GetDateBasedAsyncEnumerable<TResult>(int date, Func<IQueryable<BusinessObjectInspectionAudit>, IQueryable<TResult>> query);

        ValueTask InsertAsync(BusinessObjectInspectionAudit businessObjectInspectionAudit);
        ValueTask UpdateAsync(BusinessObjectInspectionAudit businessObjectInspectionAudit);
        ValueTask DeleteAsync(BusinessObjectInspectionAudit businessObjectInspectionAudit);
    }
}
