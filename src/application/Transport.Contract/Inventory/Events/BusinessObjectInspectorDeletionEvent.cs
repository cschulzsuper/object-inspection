﻿using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Inventory.Events;

[AllowedSubscriber(AllowedSubscribers.Inspector)]
[AllowedSubscriber(AllowedSubscribers.Notification)]
public record BusinessObjectInspectorDeletionEvent(

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