using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Events;

[AllowedSubscriber(AllowedSubscribers.Inspector)]
[AllowedSubscriber(AllowedSubscribers.Notification)]
public record BusinessObjectInspectorCreationEvent(

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    string UniqueName,

    [StringLength(140)]
    string DisplayName,

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    string Inspector)

    : EventBase;