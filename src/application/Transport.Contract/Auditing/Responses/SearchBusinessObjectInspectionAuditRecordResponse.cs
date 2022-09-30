using System.Collections.Generic;
using System.Collections.Immutable;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Responses;

public class SearchBusinessObjectInspectionAuditRecordResponse
{
    public int TotalCount { get; set; }
    public ISet<BusinessObjectInspectionAuditRecordResponse> TopResults { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditRecordResponse>();

}