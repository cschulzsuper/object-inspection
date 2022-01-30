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
    public class InspectorBusinessObjects : IStep
    {
        private readonly PaulaContext _paulaContext;
        private readonly AppSettings _appSettings;

        public InspectorBusinessObjects(
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

            var inspectorUpdateRange = new HashSet<InspectorWithId>();

            while (inspectorFeed.HasMoreResults)
            {
                foreach (var inspector in await inspectorFeed.ReadNextAsync())
                {
                    if (inspector.BusinessObjects?.Any() == true)
                    {
                        continue;
                    }

                    var businessObjectContainer2 = cosmosDatabase.GetContainer("BusinessObject");
                    var businessObjectFeed2 = businessObjectContainer2.GetItemQueryIterator<BusinessObject>($"SELECT * FROM c WHERE c.Inspector = '{inspector.UniqueName}'");

                    inspector.BusinessObjects = new HashSet<InspectorBusinessObject>();

                    while (businessObjectFeed2.HasMoreResults)
                    {
                        foreach (var businessObject in await businessObjectFeed2.ReadNextAsync())
                        {
                            inspector.BusinessObjects.Add(new InspectorBusinessObject
                            {
                                DisplayName = businessObject.DisplayName,
                                UniqueName = businessObject.UniqueName,
                                AuditSchedulePlannedAuditDate = default,
                                AuditSchedulePlannedAuditTime = default,
                                AuditScheduleDelayed = false,
                                AuditSchedulePending = false
                            });
                        }
                    }

                    inspectorUpdateRange.Add(inspector);
                }
            }

            foreach (var inspector in inspectorUpdateRange)
            {
                await inspectorContainer.ReplaceItemAsync(
                    inspector,
                    inspector.UniqueName,
                    new PartitionKey(inspector.Organization));
            }
        }

        private class InspectorWithId : Inspector
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;
        }
    }
}
