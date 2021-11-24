using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Migrator;
using Super.Paula.Data;
using Super.Paula.Environment;
using System.Threading.Tasks;
using administration = Super.Paula.Application.Administration;

namespace Super.Paula.Data.Steps
{
    public class Identity : IStep
    {
        private readonly PaulaContext _paulaContext;
        private readonly AppSettings _appSettings;

        public Identity(PaulaContext paulaContext, AppSettings appSettings)
        {
            _paulaContext = paulaContext;
            _appSettings = appSettings;
        }

        public async Task ExecuteAsync()
        {
            var cosmosClient = _paulaContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            var inspectorContainer = cosmosDatabase.GetContainer("Inspector");
            var inspectorFeed = inspectorContainer.GetItemQueryIterator<dynamic>(
                new QueryDefinition("SELECT * FROM c WHERE IS_DEFINED(c.Secret) AND IS_DEFINED(c.MailAddress) AND NOT IS_DEFINED(c.Identity)"));

            while (inspectorFeed.HasMoreResults)
            {
                foreach (var inspectorItem in await inspectorFeed.ReadNextAsync())
                {
                    var identity = new administration::Identity
                    {
                        MailAddress = inspectorItem.MailAddress,
                        Secret = inspectorItem.Secret,
                        UniqueName = inspectorItem.UniqueName
                    };

                    _paulaContext.Add(identity);

                    var inspector = new administration::Inspector
                    {
                        UniqueName = inspectorItem.UniqueName,
                        Identity = inspectorItem.UniqueName,
                        Activated = inspectorItem.Activated,
                        Organization = inspectorItem.Organization,
                        OrganizationActivated = inspectorItem.OrganizationActivated,
                        OrganizationDisplayName = inspectorItem.OrganizationDisplayName
                    };

                    _paulaContext.Update(inspector);

                }
            }

            await _paulaContext.SaveChangesAsync();
        }
    }
}
