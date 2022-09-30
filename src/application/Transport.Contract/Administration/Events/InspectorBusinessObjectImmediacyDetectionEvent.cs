using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Events;

[AllowedSubscriber(AllowedSubscribers.Notification)]
public record InspectorBusinessObjectImmediacyDetectionEvent
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
    string BusinessObjectDisplayName)

    : EventBase;