using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Events;

[AllowedSubscriber(AllowedSubscribers.Inspector)]
public record OrganizationUpdateEvent(

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    string UniqueName,

    [StringLength(140)]
    string DisplayName,

    bool Activated)

    : EventBase;