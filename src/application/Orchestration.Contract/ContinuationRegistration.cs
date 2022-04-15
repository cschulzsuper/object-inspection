using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public record ContinuationRegistration(
        string ContinuationName,
        Type ContinuationType,
        Type ContinuationHandlerType,
        IContinuationHandler ContinuationHandler,
        Func<object, ContinuationHandlerContext, Task> ContinuationHandlerCall);
}
