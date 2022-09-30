using System;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public record EventSubscription(
    string SubscriberName,
    Type EventType,
    Type EventHandlerType,
    IEventHandler EventHandler,
    Func<object, EventHandlerContext, Task> EventHandlerCall);