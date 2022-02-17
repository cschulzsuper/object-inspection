using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Super.Paula.Application.Inventory;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Super.Paula.Environment;
using Super.Paula.Application.Administration;

namespace Super.Paula.Data.Steps
{
    public class InspectorIdentity : IStep
    {
        private readonly PaulaContext _paulaContext;
        private readonly AppSettings _appSettings;

        public InspectorIdentity(
            PaulaContext paulaContext,
            AppSettings appSettings)
        {
            _paulaContext = paulaContext;
            _appSettings = appSettings;
        }

        public async Task ExecuteAsync()
        {
            var cosmosClient = _paulaContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            var inspectorContainer = cosmosDatabase.GetContainer("Inspector");
            var inspectorFeed = inspectorContainer.GetItemQueryIterator<InspectorWithId>("SELECT * FROM c");

            while (inspectorFeed.HasMoreResults)
            {
                foreach (var inspector in await inspectorFeed.ReadNextAsync())
                {
                    if (string.IsNullOrWhiteSpace(inspector.Identity))
                    {
                        continue;
                    }

                    var inspectorIdentity = new Application.Administration.IdentityInspector
                    {
                        UniqueName = inspector.Identity,
                        Inspector   = inspector.UniqueName,
                        Organization = inspector.Organization,
                        Activated = inspector.Activated && inspector.OrganizationActivated
                    };

                    _paulaContext.Add(inspectorIdentity);
                }
            }

            await _paulaContext.SaveChangesAsync();
        }

        private class InspectorWithId : Inspector
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;
        }
    }
}
