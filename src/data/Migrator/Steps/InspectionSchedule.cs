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
    public class InspectionSchedule : IStep
    {
        private readonly PaulaContext _paulaContext;
        private readonly AppSettings _appSettings;

        public InspectionSchedule(
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

                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        if (inspection.AuditSchedules == null || !inspection.AuditSchedules.Any())
                        {
                            var eightHours = TimeSpan.FromHours(8).Milliseconds;

                            inspection.AuditSchedules = new HashSet<BusinessObjectInspectionAuditSchedule>();

                            if (inspection.AuditDelayThreshold == default)
                            {
                                inspection.AuditDelayThreshold = eightHours;
                            }

                            if (inspection.AuditThreshold == default)
                            {
                                inspection.AuditThreshold = eightHours;
                            }

                            needsUpdate = true;
                        }

                        if (inspection.AssignmentDate == default)
                        {
                            var (date, time) = new DateTime(2021, 9, 1, 0, 0, 0, DateTimeKind.Utc)
                                .ToNumbers();

                            inspection.AssignmentDate = date;
                            inspection.AssignmentTime = time;

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
