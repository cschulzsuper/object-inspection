using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Events;

[AllowedSubscriber(AllowedSubscribers.Inspector)]
[AllowedSubscriber(AllowedSubscribers.Application)]
public record OrganizationDeletionEvent(

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    string UniqueName)

    : EventBase;