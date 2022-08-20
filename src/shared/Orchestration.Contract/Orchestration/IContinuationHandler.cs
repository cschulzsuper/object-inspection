using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public interface IContinuationHandler<in TContinuation> : IContinuationHandler
{
    Task HandleAsync(ContinuationHandlerContext context, TContinuation continuation);
}

public interface IContinuationHandler
{
}