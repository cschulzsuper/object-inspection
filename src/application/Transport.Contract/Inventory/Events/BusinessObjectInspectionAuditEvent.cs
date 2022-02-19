using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Events
{
    [AllowedSubscriber(AllowedSubscribers.AuditingBusinessObjectInspectionAudit)]
    public record BusinessObjectInspectionAuditEvent(

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string UniqueName,

        [StringLength(140)]
        string DisplayName,

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string AuditInspector,

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string Inspection,

        [StringLength(140)]
        string InspectionDisplayName,

        [StringLength(4000)]
        string AuditAnnotation,

        [AuditResult]
        string AuditResult,

        [DayNumber]
        int AuditDate,

        [Milliseconds]
        int AuditTime)

        : EventBase;
}