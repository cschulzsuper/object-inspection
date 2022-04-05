using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IContinuationRegistry
    {
        Func<object, ContinuationHandlerContext, Task>? GetContinuationsHandlerCall(string ContinuationsName);

        Type? GetContinuationsType(string ContinuationsName);

        void Register<TContinuation, THandler>(string ContinuationsName)
            where TContinuation : ContinuationBase
            where THandler : IContinuationHandler<TContinuation>;

        void Unregister(string ContinuationsName);
    }
}