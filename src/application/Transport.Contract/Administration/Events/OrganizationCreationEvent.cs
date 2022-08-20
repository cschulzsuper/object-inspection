using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Administration.Events;

[AllowedSubscriber(AllowedSubscribers.Application)]
public record OrganizationCreationEvent(

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    string UniqueName,

    [StringLength(140)]
    string DisplayName,

    bool Activated)

    : EventBase;