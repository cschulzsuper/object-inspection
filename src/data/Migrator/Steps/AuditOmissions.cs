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
    public class AuditOmissions : IStep
    {
        private readonly PaulaContext _paulaContext;
        private readonly AppSettings _appSettings;

        public AuditOmissions(
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

                        if (inspection.AuditSchedule.Omissions?.Any() != true)
                        {
                            inspection.AuditSchedule.Omissions = new HashSet<BusinessObjectInspectionAuditScheduleTimestamp>();

                            needsUpdate = true;
                        }

                        if (inspection.AuditSchedule.Appointments?.Any() != true)
                        {
                            inspection.AuditSchedule.Appointments = new HashSet<BusinessObjectInspectionAuditScheduleTimestamp>();

                            needsUpdate = true;
                        }

                        if (inspection.AuditSchedule.Expressions?.Any() != true)
                        {
                            inspection.AuditSchedule.Expressions = new HashSet<BusinessObjectInspectionAuditScheduleExpression>();

                            needsUpdate = true;
                        }

                        if (inspection.AuditSchedule.Additionals?.Any() != true)
                        {
                            inspection.AuditSchedule.Additionals = new HashSet<BusinessObjectInspectionAuditScheduleTimestamp>();

                            needsUpdate = true;
                        }

                        if (inspection.AuditSchedule.Delays?.Any() != true)
                        {
                            inspection.AuditSchedule.Delays = new HashSet<BusinessObjectInspectionAuditScheduleTimestamp>();

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
