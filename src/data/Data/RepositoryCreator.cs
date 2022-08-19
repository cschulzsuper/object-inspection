using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Super.Paula.Environment;

namespace Super.Paula.Data
{
    public class RepositoryCreator : IRepositoryCreator
    {
        private readonly PaulaContexts _paulaContexts;

        private readonly AppSettings _appSettings;

        public RepositoryCreator(
            PaulaContexts paulaContexts,
            AppSettings appSettings)
        {
            _paulaContexts = paulaContexts;
            _appSettings = appSettings;
        }

        public async ValueTask CreateApplicationAsync(string organization)
        {
            // Only Operation needs to be created as Application uses the same container

            var paulaOperationContext = _paulaContexts.Operation;

            var cosmosClient = paulaOperationContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            await cosmosDatabase.CreateContainerIfNotExistsAsync(organization, "/PartitionKey");
        }

        public async ValueTask DestroyApplicationAsync(string organization)
        {
            // Only Operation needs to be destroyed as Application uses the same container

            var paulaOperationContext = _paulaContexts.Operation;

            var cosmosClient = paulaOperationContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            var cosmosContainer = cosmosDatabase.GetContainer(organization);

            await cosmosContainer.DeleteContainerAsync();
        }
    }
}
