using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Requests;

public class BusinessObjectInspectionAuditScheduleRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [CronExpression]
    public string Schedule { get; set; } = string.Empty;

    [Milliseconds]
    public int Threshold { get; set; }

}