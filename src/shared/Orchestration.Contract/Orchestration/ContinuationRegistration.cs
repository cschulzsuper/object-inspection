using System;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public record ContinuationRegistration(
    Type ContinuationType,
    Type ContinuationHandlerType,
    IContinuationHandler ContinuationHandler,
    Func<object, ContinuationHandlerContext, Task> ContinuationHandlerCall);