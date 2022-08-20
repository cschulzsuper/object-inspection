using System;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public interface IContinuationRegistry
{
    Func<object, ContinuationHandlerContext, Task>? GetContinuationHandlerCall(string continuationName);

    Type? GetContinuationType(string continuationName);

    void Register<TContinuation, THandler>()
        where TContinuation : ContinuationBase
        where THandler : IContinuationHandler<TContinuation>;

    void Unregister<TContinuation>()
        where TContinuation : ContinuationBase;
}