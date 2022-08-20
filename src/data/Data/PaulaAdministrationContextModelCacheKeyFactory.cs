using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Super.Paula.Data;

public class PaulaAdministrationContextModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
        => (context.GetType(), designTime);

    public object Create(DbContext context)
        => Create(context, false);
}