using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Super.Paula.Application.Auditing.Responses
{
    public class BusinessObjectInspectionAuditScheduleResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ETag { get; set; } = null;

        public ISet<BusinessObjectInspectionAuditScheduleExpressionResponse> Expressions { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleExpressionResponse>();
        public int Threshold { get; set; }

        public ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> Omissions { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleTimestampResponse>();
        public ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> Additionals { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleTimestampResponse>();
        public ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> Appointments { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleTimestampResponse>();
    }
}
