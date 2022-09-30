using System.Text.Json.Serialization;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Responses;

public class ReplaceBusinessObjectInspectionAuditScheduleResponse : BusinessObjectInspectionAuditScheduleResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ETag { get; set; } = null;
}