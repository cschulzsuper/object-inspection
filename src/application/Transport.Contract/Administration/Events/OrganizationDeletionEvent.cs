using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Administration.Events;

[AllowedSubscriber(AllowedSubscribers.Inspector)]
[AllowedSubscriber(AllowedSubscribers.Application)]
public record OrganizationDeletionEvent(

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    string UniqueName)

    : EventBase;