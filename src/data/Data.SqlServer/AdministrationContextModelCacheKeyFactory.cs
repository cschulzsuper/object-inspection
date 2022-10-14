using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChristianSchulz.ObjectInspection.Data;

public class AdministrationContextModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
        => (context.GetType(), designTime);

    public object Create(DbContext context)
        => Create(context, false);
}