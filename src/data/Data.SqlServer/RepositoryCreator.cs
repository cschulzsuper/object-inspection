using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChristianSchulz.ObjectInspection.Data;

public class RepositoryCreator : IRepositoryCreator
{
    private readonly ObjectInspectionContexts _objectInspectionContexts;

    public RepositoryCreator(ObjectInspectionContexts objectInspectionContexts)
    {
        _objectInspectionContexts = objectInspectionContexts;
    }

    public async ValueTask CreateApplicationAsync(string organization)
    {
        var operationContext = _objectInspectionContexts.Operation;
        operationContext.State.CurrentOrganization = organization;

        var operationCreator = operationContext.GetService<IRelationalDatabaseCreator>();
        await operationCreator.CreateTablesAsync();

        var applicationContext = _objectInspectionContexts.Application;
        applicationContext.State.CurrentOrganization = organization;

        var applicationCreator = applicationContext.GetService<IRelationalDatabaseCreator>();
        await applicationCreator.CreateTablesAsync();
    }

    public async ValueTask DestroyApplicationAsync(string organization)
    {
        // var operationContext = _objectInspectionContexts.Operation;
        // await operationContext.Database.EnsureDeletedAsync();

        await Task.CompletedTask;
    }
}