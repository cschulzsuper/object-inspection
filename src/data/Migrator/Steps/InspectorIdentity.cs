using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Super.Paula.Application.Administration;
using Super.Paula.Environment;
using System.Threading.Tasks;

namespace Super.Paula.Data.Steps
{
    public class InspectorIdentity : IStep
    {
        private readonly PaulaApplicationContext _paulaApplicationContext;
        private readonly PaulaAdministrationContext _paulaAdministrationContext;
        private readonly AppSettings _appSettings;

        public InspectorIdentity(
            PaulaApplicationContext paulaApplicationContext,
            PaulaAdministrationContext paulaAdministrationContext,
            AppSettings appSettings)
        {
            _paulaApplicationContext = paulaApplicationContext;
            _paulaAdministrationContext = paulaAdministrationContext;
            _appSettings = appSettings;
        }

        public async Task ExecuteAsync()
        {
            var cosmosClient = _paulaApplicationContext.Database.GetCosmosClient();
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

                    var existing = _paulaAdministrationContext.Set<IdentityInspector>().Find(inspector.Identity, inspector.Organization, inspector.UniqueName) != null;

                    if (!existing)
                    {
                        var inspectorIdentity = new Application.Administration.IdentityInspector
                        {
                            UniqueName = inspector.Identity,
                            Inspector   = inspector.UniqueName,
                            Organization = inspector.Organization,
                            Activated = inspector.Activated && inspector.OrganizationActivated
                        };

                        _paulaAdministrationContext.Add(inspectorIdentity);
                    }
                }
            }

            await _paulaAdministrationContext.SaveChangesAsync();
        }

        private class InspectorWithId : Inspector
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;
        }
    }
}
