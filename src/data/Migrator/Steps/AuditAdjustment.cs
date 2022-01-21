using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Super.Paula.Application.Inventory;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Super.Paula.Environment;

namespace Super.Paula.Data.Steps
{
    public class AuditAdjustment : IStep
    {
        private readonly PaulaContext _paulaContext;
        private readonly AppSettings _appSettings;

        public AuditAdjustment(
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

            var businessObjectContainer = cosmosDatabase.GetContainer("BusinessObject");
            var businessObjectFeed = businessObjectContainer.GetItemQueryIterator<BusinessObjectWithId>("SELECT * FROM c");

            var businessObjectUpdateRange = new HashSet<BusinessObjectWithId>();

            while (businessObjectFeed.HasMoreResults)
            {
                foreach (var businessObject in await businessObjectFeed.ReadNextAsync())
                {
                    foreach (var inspection in businessObject.Inspections)
                    {
                        var needsUpdate = false;

                        if (inspection.AuditScheduleDrops == null ||
                            !inspection.AuditScheduleDrops.Any())
                        {
                            inspection.AuditScheduleDrops = new HashSet<BusinessObjectInspectionAuditScheduleDrop>();

                            needsUpdate = true;
                        }


                        if (inspection.AuditScheduleSupplements == null ||
                            !inspection.AuditScheduleSupplements.Any())
                        {
                            inspection.AuditScheduleSupplements = new HashSet<BusinessObjectInspectionAuditScheduleSupplement>();

                            needsUpdate = true;
                        }

                        if (needsUpdate)
                        {
                            businessObjectUpdateRange.Add(businessObject);
                        }
                    }
                }

                foreach (var businessObject in businessObjectUpdateRange)
                {
                    await businessObjectContainer.ReplaceItemAsync(
                        businessObject,
                        businessObject.UniqueName,
                        new PartitionKey(businessObject.Organization));
                }
            }
        }

        private class BusinessObjectWithId : BusinessObject
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;

            public string Organization { get; set; } = string.Empty;
        }
    }
}
