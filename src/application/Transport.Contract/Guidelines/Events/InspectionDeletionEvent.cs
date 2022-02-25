using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Guidelines.Events
{
    [AllowedSubscriber(AllowedSubscribers.AuditingBusinessObjectInspectionAuditRecord)]
    [AllowedSubscriber(AllowedSubscribers.AuditingBusinessObjectInspection)]
    public record InspectionDeletionEvent(

        [StringLength(140)]
        [UniqueName]
        [KebabCase]
        string UniqueName)

        : EventBase;
}