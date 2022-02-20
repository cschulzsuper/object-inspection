using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Authorization;
using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Data.Steps
{
    public class InitializationApplication : IStep
    {
        private readonly PaulaAdministrationContext _paulaAdministrationContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly AppSettings _appSettings;

        public InitializationApplication(
            PaulaAdministrationContext paulaAdministrationContext, 
            IServiceProvider serviceProvider, AppSettings appSettings)
        {
            _paulaAdministrationContext = paulaAdministrationContext;
            _serviceProvider = serviceProvider;
            _appSettings = appSettings;
        }

        public async Task ExecuteAsync()
        {
            var organizations = _paulaAdministrationContext
                .Set<Organization>()
                .Select(x => x.UniqueName)
                .ToList();

            foreach(var organization in organizations)
            {
                await InitializationApplicationContextAsync(organization);
                await InitializationBusinessObjectsAsync(organization);
                await InitializationInspectionsAsync(organization);
                await InitializationInspectorAsync(organization);
                await InitializationNotificationAsync(organization);
                await InitializationBusinessObjectInspectionAuditAsync(organization);
            }
        }

        private Task InitializationApplicationContextAsync(string organization)
        {
            using var scope = _serviceProvider.CreateScope();
            SetupScope(organization, scope);

            var paulaApplicationContext = scope.ServiceProvider.GetRequiredService<PaulaApplicationContext>();

            paulaApplicationContext.Database.EnsureCreated();

            return Task.CompletedTask;
        }

        private async Task InitializationBusinessObjectsAsync(string organization)
        {
            using var scope = _serviceProvider.CreateScope();
            SetupScope(organization, scope);

            var paulaApplicationContext = scope.ServiceProvider.GetRequiredService<PaulaApplicationContext>();

            var cosmosClient = _paulaAdministrationContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            var businessObjectContainer = cosmosDatabase.GetContainer("BusinessObject");
            var businessObjectFeed = businessObjectContainer.GetItemQueryIterator<BusinessObjectWithId>($"SELECT * FROM c WHERE c.Organization = '{organization}'");

            while (businessObjectFeed.HasMoreResults)
            {
                foreach (var businessObject in await businessObjectFeed.ReadNextAsync())
                {
                    var existing = paulaApplicationContext.Set<BusinessObject>().Find("business-object", businessObject.UniqueName) != null;

                    if (!existing)
                    {
                        paulaApplicationContext.Set<BusinessObject>().Add(businessObject);
                    }
                }
            }

            await paulaApplicationContext.SaveChangesAsync();
        }

        private async Task InitializationInspectionsAsync(string organization)
        {
            using var scope = _serviceProvider.CreateScope();
            SetupScope(organization, scope);

            var paulaApplicationContext = scope.ServiceProvider.GetRequiredService<PaulaApplicationContext>();

            var cosmosClient = _paulaAdministrationContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            var inspectionContainer = cosmosDatabase.GetContainer("Inspection");
            var inspectionFeed = inspectionContainer.GetItemQueryIterator<InspectionWithId>($"SELECT * FROM c WHERE c.Organization = '{organization}'");

            while (inspectionFeed.HasMoreResults)
            {
                foreach (var inspection in await inspectionFeed.ReadNextAsync())
                {
                    var existing = paulaApplicationContext.Set<Inspection>().Find("inspection", inspection.UniqueName) != null;

                    if (!existing)
                    {
                        paulaApplicationContext.Set<Inspection>().Add(inspection);
                    }
                }
            }

            await paulaApplicationContext.SaveChangesAsync();
        }

        private async Task InitializationInspectorAsync(string organization)
        {
            using var scope = _serviceProvider.CreateScope();
            SetupScope(organization, scope);

            var paulaApplicationContext = scope.ServiceProvider.GetRequiredService<PaulaApplicationContext>();

            var cosmosClient = _paulaAdministrationContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            var inspectorContainer = cosmosDatabase.GetContainer("Inspector");
            var inspectorFeed = inspectorContainer.GetItemQueryIterator<InspectorWithId>($"SELECT * FROM c WHERE c.Organization = '{organization}'");

            while (inspectorFeed.HasMoreResults)
            {
                foreach (var inspector in await inspectorFeed.ReadNextAsync())
                {
                    var existing = paulaApplicationContext.Set<Inspector>().Find("inspector", inspector.UniqueName) != null;

                    if (!existing)
                    {
                        paulaApplicationContext.Set<Inspector>().Add(inspector);
                    }
                }
            }

            await paulaApplicationContext.SaveChangesAsync();
        }

        private async Task InitializationNotificationAsync(string organization)
        {
            using var scope = _serviceProvider.CreateScope();
            SetupScope(organization, scope);

            var paulaApplicationContext = scope.ServiceProvider.GetRequiredService<PaulaApplicationContext>();

            var cosmosClient = _paulaAdministrationContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            var notificationContainer = cosmosDatabase.GetContainer("Notification");
            var notificationFeed = notificationContainer.GetItemQueryIterator<NotificationWithId>($"SELECT * FROM c WHERE STARTSWITH(c.PartitionKey,'{organization}/',false)");

            while (notificationFeed.HasMoreResults)
            {
                foreach (var notification in await notificationFeed.ReadNextAsync())
                {
                    var existing = paulaApplicationContext.Set<Notification>().Find(
                        $"notification/{notification.Inspector}", 
                        notification.Date, 
                        notification.Time) != null;

                    if (!existing)
                    {
                        paulaApplicationContext.Set<Notification>().Add(notification);
                    }
                }
            }

            await paulaApplicationContext.SaveChangesAsync();
        }

        private async Task InitializationBusinessObjectInspectionAuditAsync(string organization)
        {
            using var scope = _serviceProvider.CreateScope();
            SetupScope(organization, scope);

            var paulaApplicationContext = scope.ServiceProvider.GetRequiredService<PaulaApplicationContext>();

            var cosmosClient = _paulaAdministrationContext.Database.GetCosmosClient();
            var cosmosDatabase = cosmosClient.GetDatabase(_appSettings.CosmosDatabase);

            var businessObjectInspectionAuditContainer = cosmosDatabase.GetContainer("BusinessObjectInspectionAudit");
            var businessObjectInspectionAuditFeed = businessObjectInspectionAuditContainer.GetItemQueryIterator<BusinessObjectInspectionAuditWithId>(
                $"SELECT * FROM c WHERE STARTSWITH(c.PartitionKey,'{organization}/',false)");

            while (businessObjectInspectionAuditFeed.HasMoreResults)
            {
                foreach (var businessObjectInspectionAudit in await businessObjectInspectionAuditFeed.ReadNextAsync())
                {
                    var existing = paulaApplicationContext.Set<BusinessObjectInspectionAudit>().Find(
                        "business-object-inspection-audit/{businessObjectInspectionAudit.AuditDate}", 
                        businessObjectInspectionAudit.BusinessObject,
                        businessObjectInspectionAudit.Inspection,
                        businessObjectInspectionAudit.AuditTime) != null;

                    if (!existing)
                    {
                        paulaApplicationContext.Set<BusinessObjectInspectionAudit>().Add(businessObjectInspectionAudit);
                    }
                }
            }

            await paulaApplicationContext.SaveChangesAsync();
        }

        private static void SetupScope(string organization, IServiceScope scope)
        {
            scope.ServiceProvider.ConfigureUser(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                                new Claim("Organization", organization)
                        })));

            var paulaContextState = scope.ServiceProvider.GetRequiredService<PaulaContextState>();
            var user = scope.ServiceProvider.GetRequiredService<ClaimsPrincipal>();

            paulaContextState.CurrentOrganization = user.HasOrganization()
               ? user.GetOrganization()
               : string.Empty;

            paulaContextState.CurrentInspector = user.HasInspector()
               ? user.GetInspector()
               : string.Empty;
        }

        private class BusinessObjectWithId : BusinessObject
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;
        }

        private class InspectionWithId : Inspection
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;
        }

        private class InspectorWithId : Inspector
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;
        }

        private class BusinessObjectInspectionAuditWithId : BusinessObjectInspectionAudit
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;
        }

        private class NotificationWithId : Notification
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;
        }
    }
}
