using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Environment;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Inventory.Events;
using System;

namespace Super.Paula.Application.Administration
{
    public class InspectorHandler : 
        IInspectorHandler,
        IInspectorEventHandler
    {
        private readonly IInspectorManager _inspectorManager;
        private readonly IOrganizationProvider _organizationProvider;
        private readonly IIdentityInspectorManager _identityInspectorManager;
        private readonly AppState _appState;
        private readonly IEventBus _eventBus;

        private Func<string, InspectorBusinessObjectResponse, Task>? _onBusinessObjectCreationHandler;
        private Func<string, InspectorBusinessObjectResponse, Task>? _onBusinessObjectUpdateHandler;
        private Func<string, string, Task>? _onBusinessObjectDeletionHandler;

        public InspectorHandler(
            IInspectorManager inspectorManager,
            IOrganizationProvider organizationProvider,
            IIdentityInspectorManager identityInspectorManager,
            AppState appState,
            IEventBus eventBus) 
        {
            _inspectorManager = inspectorManager;
            _organizationProvider = organizationProvider;
            _identityInspectorManager = identityInspectorManager;
            _appState = appState;
            _eventBus = eventBus;
        }

        public async ValueTask<InspectorResponse> CreateAsync(InspectorRequest request)
        {
            var organization = await _organizationProvider.GetAsync(_appState.CurrentOrganization);

            var entity = new Inspector
            {
                Identity = request.Identity,
                UniqueName = request.UniqueName,
                Activated = request.Activated,
                Organization = organization.UniqueName,
                OrganizationActivated = organization.Activated,
                OrganizationDisplayName = organization.DisplayName
            };

            await _inspectorManager.InsertAsync(entity);

            var identity = new IdentityInspector
            {
                Activated = entity.Activated && organization.Activated,
                Inspector = entity.UniqueName,
                Organization = organization.UniqueName,
                UniqueName = entity.Identity
            };

            await _identityInspectorManager.InsertAsync(identity);

            return new InspectorResponse
            {
                Identity = entity.Identity,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated,
                BusinessObjects = entity.BusinessObjects.ToResponse()
            };
        }

        public async ValueTask DeleteAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);
            await _inspectorManager.DeleteAsync(entity);

            var identity = await _identityInspectorManager.GetAsync(
                entity.Identity,
                entity.Organization,
                entity.UniqueName);
            await _identityInspectorManager.DeleteAsync(identity);
        }

        public IAsyncEnumerable<InspectorResponse> GetAll()
            => _inspectorManager
                .GetAsyncEnumerable(query => query
                .Select(entity => new InspectorResponse
                {
                    Identity = entity.Identity,
                    UniqueName = entity.UniqueName,
                    Activated = entity.Activated,
                    BusinessObjects = entity.BusinessObjects.ToResponse()
                }));

        public async ValueTask<InspectorResponse> GetAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);
         
            return new InspectorResponse
            {
                Identity = entity.Identity,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated,
                BusinessObjects = entity.BusinessObjects.ToResponse()
            };
        }

        public async ValueTask<InspectorResponse> GetCurrentAsync()
        {
            var entity = await _inspectorManager.GetAsync(_appState.CurrentInspector);

            return new InspectorResponse
            {
                Identity = entity.Identity,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated,
                BusinessObjects = entity.BusinessObjects.ToResponse()
            };
        }

        public async ValueTask ReplaceAsync(string inspector, InspectorRequest request)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            var oldIdentity = entity.Identity;

            entity.Identity = request.Identity;
            entity.UniqueName = request.UniqueName;
            entity.Activated = request.Activated;

            await _inspectorManager.UpdateAsync(entity);

            var identity = await _identityInspectorManager.GetAsync(
                oldIdentity,
                entity.Organization,
                entity.UniqueName);

            await _identityInspectorManager.DeleteAsync(identity);

            identity = new IdentityInspector
            {
                Activated = entity.Activated && entity.OrganizationActivated,
                Inspector = entity.UniqueName,
                Organization = entity.Organization,
                UniqueName = entity.Identity
            };

            await _identityInspectorManager.InsertAsync(identity);
        }

        public async ValueTask ActivateAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            entity.Activated = true;

            await _inspectorManager.UpdateAsync(entity);

            var identity = await _identityInspectorManager.GetAsync(
                entity.Identity,
                entity.Organization,
                entity.UniqueName);

            identity.Activated = entity.OrganizationActivated;

            await _identityInspectorManager.UpdateAsync(identity);
        }

        public async ValueTask DeactivateAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            entity.Activated = false;

            await _inspectorManager.UpdateAsync(entity);

            var identity = await _identityInspectorManager.GetAsync(
                entity.Identity,
                entity.Organization,
                entity.UniqueName);

            identity.Activated = false;

            await _identityInspectorManager.UpdateAsync(identity);
        }

        public IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization)
            => _inspectorManager
               .GetAsyncEnumerable(query => query
                   .Where(x => x.Organization == organization)
                   .Select(entity => new InspectorResponse
                   {
                       Identity = entity.Identity,
                       UniqueName = entity.UniqueName,
                       Activated = entity.Activated,
                       BusinessObjects = entity.BusinessObjects.ToResponse()
                   }));

        public IAsyncEnumerable<IdentityInspectorResponse> GetAllForIdentity(string identity)
            => _identityInspectorManager
                .GetIdentityBasedAsyncEnumerable(identity,
                    query => query
                       .Select(entity => new IdentityInspectorResponse
                       {
                           Identity = entity.UniqueName,
                           UniqueName = entity.Inspector,
                           Activated = entity.Activated,
                           Organization = entity.Organization
                       }));

        public async ValueTask ProcessAsync(string organization, OrganizationEvent @event)
        {
            var inspectors = _inspectorManager.GetQueryable()
                .Where(x => x.Organization == organization)
                .ToList();

            foreach (var inspector in inspectors)
            {
                inspector.OrganizationActivated =  @event.Activated ?? inspector.OrganizationActivated;
                inspector.OrganizationDisplayName =  @event.DisplayName ?? inspector.OrganizationDisplayName;

                await _inspectorManager.UpdateAsync(inspector);

                var identity = await _identityInspectorManager.GetAsync(
                    inspector.Identity,
                    inspector.Organization,
                    inspector.UniqueName);

                identity.Activated = inspector.Activated && inspector.OrganizationActivated;

                await _identityInspectorManager.UpdateAsync(identity);
            }
        }

        public async ValueTask ProcessAsync(string businessObject, BusinessObjectInspectorEvent @event)
        {
            if (@event.OldInspector != null &&
                @event.OldInspector == @event.NewInspector)
            {
                var oldInspector = _inspectorManager
                    .GetQueryable()
                    .SingleOrDefault(x => x.UniqueName == @event.OldInspector);

                if (oldInspector != null)
                {
                    var oldBusinessObjects = oldInspector.BusinessObjects
                        .Where(x => x.UniqueName == businessObject);

                    foreach (var oldBusinessObject in oldBusinessObjects.Skip(1))
                    {
                        oldInspector.BusinessObjects.Remove(oldBusinessObject);
                    }

                    var changedBusinessObject = oldBusinessObjects.First();
                    changedBusinessObject.DisplayName = @event.BusinessObjectDisplayName ?? changedBusinessObject.DisplayName;

                    await _inspectorManager.UpdateAsync(oldInspector);

                    var onBusinessObjectUpdateTask = _onBusinessObjectUpdateHandler?.Invoke(@event.OldInspector, changedBusinessObject.ToResponse());
                    if (onBusinessObjectUpdateTask != null) await onBusinessObjectUpdateTask;
                }
            }

            var useBusinessObject = (InspectorBusinessObject?)null;

            if (@event.OldInspector != null &&
                @event.OldInspector != @event.NewInspector)
            {
                var oldInspector = _inspectorManager
                    .GetQueryable()
                    .SingleOrDefault(x => x.UniqueName == @event.OldInspector);

                if (oldInspector != null)
                {
                    var oldBusinessObjects = oldInspector.BusinessObjects
                        .Where(x => x.UniqueName == businessObject);

                    useBusinessObject = oldBusinessObjects.FirstOrDefault();

                    foreach (var oldBusinessObject in oldBusinessObjects)
                    {
                        oldInspector.BusinessObjects.Remove(oldBusinessObject);
                    }

                    await _inspectorManager.UpdateAsync(oldInspector);

                    var onBusinessObjectDeletionTask = _onBusinessObjectDeletionHandler?.Invoke(oldInspector.UniqueName, businessObject);
                    if (onBusinessObjectDeletionTask != null) await onBusinessObjectDeletionTask;
                }
            }

            if (@event.OldInspector != @event.NewInspector &&
                @event.NewInspector != null)
            {
                var newInspector = _inspectorManager
                    .GetQueryable()
                    .SingleOrDefault(x => x.UniqueName == @event.NewInspector);

                if (newInspector != null)
                {
                    var newBusinessObject = useBusinessObject ?? new InspectorBusinessObject();

                    newBusinessObject.DisplayName = @event.BusinessObjectDisplayName ?? string.Empty;
                    newBusinessObject.UniqueName = businessObject;

                    newInspector.BusinessObjects.Add(newBusinessObject);

                    await _inspectorManager.UpdateAsync(newInspector);

                    var onBusinessObjectCreationTask = _onBusinessObjectCreationHandler?.Invoke(newInspector.UniqueName, newBusinessObject.ToResponse());
                    if (onBusinessObjectCreationTask != null) await onBusinessObjectCreationTask;
                }
            }
        }

        public async ValueTask ProcessAsync(string businessObject, BusinessObjectInspectionAuditScheduleEvent @event)
        {
            if (@event.Inspector == null)
            {
                return;
            }

            var inspector = _inspectorManager
                .GetQueryable()
                .SingleOrDefault(x => x.UniqueName == @event.Inspector);

            if (inspector == null)
            {
                return;
            }

            var inspectorBusinessObject = inspector.BusinessObjects
                .Single(x => x.UniqueName == businessObject);

            if (@event.PlannedAuditDate == null ||
                @event.PlannedAuditTime == null ||
                @event.PlannedAuditDate.Value == default )
            {
                inspectorBusinessObject.AuditSchedulePlannedAuditDate = default;
                inspectorBusinessObject.AuditSchedulePlannedAuditTime = default;
                inspectorBusinessObject.AuditScheduleDelayed = false;
                inspectorBusinessObject.AuditSchedulePending = false;

                await _inspectorManager.UpdateAsync(inspector);
            } 
            else
            {
                var plannedAuditTimestamp = (@event.PlannedAuditDate.Value, @event.PlannedAuditTime.Value)
                    .ToDateTime();

                var oldDelayed = inspectorBusinessObject.AuditScheduleDelayed;
                var oldPending = inspectorBusinessObject.AuditSchedulePending;

                inspectorBusinessObject.AuditSchedulePlannedAuditDate = @event.PlannedAuditDate.Value;
                inspectorBusinessObject.AuditSchedulePlannedAuditTime = @event.PlannedAuditTime.Value;

                var now = DateTime.UtcNow;
                var threshold = @event.Threshold ?? 0;

                inspectorBusinessObject.AuditScheduleDelayed = now > plannedAuditTimestamp.AddMilliseconds(threshold);
                inspectorBusinessObject.AuditSchedulePending = now > plannedAuditTimestamp.AddMilliseconds(-threshold);

                await _inspectorManager.UpdateAsync(inspector);

                await PublishInspectorBusinessObjectAsync(inspector, inspectorBusinessObject, oldDelayed, oldPending);

                var onBusinessObjectUpdateTask = _onBusinessObjectUpdateHandler?.Invoke(@event.Inspector, inspectorBusinessObject.ToResponse());
                if (onBusinessObjectUpdateTask != null) await onBusinessObjectUpdateTask;
            }
        }

        private async ValueTask PublishInspectorBusinessObjectAsync(
            Inspector inspector, 
            InspectorBusinessObject inspectorBusinessObject,
            bool oldDelayed,
            bool pldPending)
        {
            var @event = new InspectorBusinessObjectEvent
            {
                UniqueName = inspectorBusinessObject.UniqueName,
                DisplayName = inspectorBusinessObject.DisplayName,
                NewAuditScheduleDelayed = inspectorBusinessObject.AuditScheduleDelayed,
                NewAuditSchedulePending = inspectorBusinessObject.AuditSchedulePending,
                OldAuditScheduleDelayed = oldDelayed,
                OldAuditSchedulePending = pldPending
            };

            await _eventBus.PublishAsync(EventCategories.Notification, inspector.UniqueName, @event);
        }

        public Task<IDisposable> OnBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
        {
            _onBusinessObjectCreationHandler = handler;
            return Task.FromResult<IDisposable>(null!);
        }

        public Task<IDisposable> OnBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
        {
            _onBusinessObjectUpdateHandler = handler;
            return Task.FromResult<IDisposable>(null!);
        }

        public Task<IDisposable> OnBusinessObjectDeletionAsync(Func<string, string, Task> handler)
        {
            _onBusinessObjectDeletionHandler = handler;
            return Task.FromResult<IDisposable>(null!);
        }
    }
}