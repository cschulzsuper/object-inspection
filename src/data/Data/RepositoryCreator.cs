using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChristianSchulz.ObjectInspection.Shared.Environment;

namespace ChristianSchulz.ObjectInspection.Data;

public class RepositoryCreator : IRepositoryCreator
{
    private readonly ObjectInspectionContexts _objectInspectionContexts;

    private readonly AppSettings _appSettings;

    public RepositoryCreator(
        ObjectInspectionContexts objectInspectionContexts,
        AppSettings appSettings)
    {
        _objectInspectionContexts = objectInspectionContexts;
        _appSettings = appSettings;
    }

    public async ValueTask CreateApplicationAsync(string organization)
    {
        // Only Operation needs to be created as Application uses the same container

        var operationContext = _objectInspectionContexts.Operation;

        var cosmosClient = operationContext.Database.GetCosmosClient();
        var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

        await cosmosDatabase.CreateContainerIfNotExistsAsync(organization, "/partitionKey");
    }

    public async ValueTask DestroyApplicationAsync(string organization)
    {
        // Only Operation needs to be destroyed as Application uses the same container

        var operationContext = _objectInspectionContexts.Operation;

        var cosmosClient = operationContext.Database.GetCosmosClient();
        var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

        var cosmosContainer = cosmosDatabase.GetContainer(organization);

        await cosmosContainer.DeleteContainerAsync();
    }
}