using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Administration.Events
{
    [AllowedSubscriber(AllowedSubscribers.Inspector)]
    public record OrganizationUpdateEvent(

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string UniqueName,

        [StringLength(140)]
        string DisplayName,

        bool Activated)

        : EventBase("event-organization-update");
}