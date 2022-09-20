﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChristianSchulz.ObjectInspection.Data;

public class ApplicationContextModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
        => context is ObjectInspectionContext objectInspectionContext
            ? (context.GetType(), objectInspectionContext.State.CurrentOrganization, objectInspectionContext.State.ExtensionModelIndicator, designTime)
            : (context.GetType(), designTime);

    public object Create(DbContext context)
        => Create(context, false);
}