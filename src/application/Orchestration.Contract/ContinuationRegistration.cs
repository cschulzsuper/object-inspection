using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public record ContinuationRegistration(
        Type ContinuationType,
        Type ContinuationHandlerType,
        IContinuationHandler ContinuationHandler,
        Func<object, ContinuationHandlerContext, Task> ContinuationHandlerCall);
}
