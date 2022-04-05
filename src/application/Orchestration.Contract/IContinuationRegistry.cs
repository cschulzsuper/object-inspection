using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IContinuationRegistry
    {
        Func<object, ContinuationHandlerContext, Task>? GetContinuationHandlerCall(string continuationName);

        Type? GetContinuationType(string continuationName);

        void Register<TContinuation, THandler>(string continuationName)
            where TContinuation : ContinuationBase
            where THandler : IContinuationHandler<TContinuation>;

        void Unregister(string continuationName);
    }
}