using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Administration.Events
{
    [AllowedSubscriber(AllowedSubscribers.CommunicationNotification)]
    public record InspectorBusinessObjectEvent
    (
        [StringLength(140)]
        [UniqueName]
        [KebabCase]
        string UniqueName,

        [StringLength(140)]
        [UniqueName]
        [KebabCase]
        string BusinessObject,

        [StringLength(140)]
        string BusinessObjectDisplayName,

        bool NewAuditSchedulePending,

        bool OldAuditSchedulePending,

        bool NewAuditScheduleDelayed,

        bool OldAuditScheduleDelayed)

        : EventBase;

}