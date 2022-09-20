using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public interface IBusinessObjectInspectionAuditRecordManager
{
    ValueTask<BusinessObjectInspectionAuditRecord> GetAsync(string businessObject, string inspection, int date, int time);

    ValueTask<BusinessObjectInspectionAuditRecord?> GetOrDefaultAsync(string businessObject, string inspection, int date, int time);

    IQueryable<BusinessObjectInspectionAuditRecord> GetQueryable();

    IAsyncEnumerable<BusinessObjectInspectionAuditRecord> GetAsyncEnumerable(
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObjectInspectionAuditRecord>, IQueryable<TResult>> query,
        CancellationToken cancellationToken = default);

    IQueryable<BusinessObjectInspectionAuditRecord> GetDateBasedQueryable(int date);
    IAsyncEnumerable<BusinessObjectInspectionAuditRecord> GetDateBasedAsyncEnumerable(int date);
    IAsyncEnumerable<TResult> GetDateBasedAsyncEnumerable<TResult>(int date, Func<IQueryable<BusinessObjectInspectionAuditRecord>, IQueryable<TResult>> query);

    ValueTask InsertAsync(BusinessObjectInspectionAuditRecord businessObjectInspectionAuditRecord);
    ValueTask UpdateAsync(BusinessObjectInspectionAuditRecord businessObjectInspectionAuditRecord);
    ValueTask DeleteAsync(BusinessObjectInspectionAuditRecord businessObjectInspectionAuditRecord);
}