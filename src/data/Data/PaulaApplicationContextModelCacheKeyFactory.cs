using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Super.Paula.Data
{
    public class PaulaApplicationContextModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context, bool designTime)
            => context is PaulaContext paulaContext
                ? (context.GetType(), paulaContext.State.CurrentOrganization, paulaContext.State.ExtensionModelIndicator, designTime)
                : context.GetType();

        public object Create(DbContext context)
            => Create(context, false);
    }
}
