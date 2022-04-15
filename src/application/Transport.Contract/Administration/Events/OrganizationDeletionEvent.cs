using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Administration.Events
{
    [AllowedSubscriber(AllowedSubscribers.Inspector)]
    [AllowedSubscriber(AllowedSubscribers.Application)]
    public record OrganizationDeletionEvent(

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string UniqueName)

        : EventBase("event-organization-deletion");
}