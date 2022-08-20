using System;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public record EventSubscription(
    string SubscriberName,
    Type EventType,
    Type EventHandlerType,
    IEventHandler EventHandler,
    Func<object, EventHandlerContext, Task> EventHandlerCall);