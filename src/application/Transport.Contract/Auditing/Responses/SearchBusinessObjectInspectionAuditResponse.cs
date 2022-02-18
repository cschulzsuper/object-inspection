using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Paula.Application.Auditing.Responses
{
    public class SearchBusinessObjectInspectionAuditResponse
    {
        public int TotalCount { get; set; }
        public ISet<BusinessObjectInspectionAuditResponse> TopResults { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditResponse>();

    }
}