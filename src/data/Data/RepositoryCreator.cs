using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Super.Paula.Environment;

namespace Super.Paula.Data
{
    public class RepositoryCreator : IRepositoryCreator
    {
        private readonly PaulaApplicationContext _paulaApplicationContext;
        private readonly PaulaContextState _paulaContextState;
        private readonly AppSettings _appSettings;

        public RepositoryCreator(
            PaulaApplicationContext paulaApplicationContext,
            PaulaContextState paulaContextState,
            AppSettings appSettings)
        {
            _paulaApplicationContext = paulaApplicationContext;
            _paulaContextState = paulaContextState;
            _appSettings = appSettings;
        }

        public async ValueTask CreateApplicationAsync()
        {
            var cosmosClient = _paulaApplicationContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            await cosmosDatabase.CreateContainerIfNotExistsAsync(_paulaContextState.CurrentOrganization, "/PartitionKey");
        }

        public async ValueTask DestroyApplicationAsync()
        {
            var cosmosClient = _paulaApplicationContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            var cosmosContainer = cosmosDatabase.GetContainer(_paulaContextState.CurrentOrganization);

            await cosmosContainer.DeleteContainerAsync();
        }
    }
}
