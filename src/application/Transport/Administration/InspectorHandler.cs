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
        private readonly AppState _appState;
        private readonly IEventBus _eventBus;

        public InspectorHandler(
            IInspectorManager inspectorManager,
            IOrganizationProvider organizationProvider,
            AppState appState,
            IEventBus eventBus) 
        {
            _inspectorManager = inspectorManager;
            _organizationProvider = organizationProvider;
            _appState = appState;
            _eventBus = eventBus;
        }

        public async ValueTask<InspectorResponse> CreateAsync(InspectorRequest request)
        {
            var organization = await _organizationProvider.GetAsync(_appState.CurrentOrganization);

            var entity = new Inspector
            {
                Identity = request.UniqueName,
                UniqueName = request.UniqueName,
                Activated = request.Activated,
                Organization = organization.UniqueName,
                OrganizationActivated = organization.Activated,
                OrganizationDisplayName = organization.DisplayName
            };

            await _inspectorManager.InsertAsync(entity);

            return new InspectorResponse
            {
                Identity = entity.Identity,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated
            };
        }

        public async ValueTask DeleteAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            await _inspectorManager.DeleteAsync(entity);
        }

        public IAsyncEnumerable<InspectorResponse> GetAll()
            => _inspectorManager
                .GetAsyncEnumerable(query => query
                .Select(entity => new InspectorResponse
                {
                    Identity = entity.Identity,
                    UniqueName = entity.UniqueName,
                    Activated = entity.Activated
                }));

        public async ValueTask<InspectorResponse> GetAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);
         
            return new InspectorResponse
            {
                Identity = entity.Identity,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated
            };
        }

        public async ValueTask ReplaceAsync(string inspector, InspectorRequest request)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            entity.Identity = request.Identity;
            entity.UniqueName = request.UniqueName;
            entity.Activated = request.Activated;

            await _inspectorManager.UpdateAsync(entity);
        }

        public async ValueTask ActivateAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            entity.Activated = true;

            await _inspectorManager.UpdateAsync(entity);
        }

        public async ValueTask DeactivateAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            entity.Activated = false;

            await _inspectorManager.UpdateAsync(entity);
        }

        public IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization)
        {
            return _inspectorManager
               .GetAsyncEnumerable(query => query
                   .Where(x => x.Organization == organization)
                   .Select(entity => new InspectorResponse
                   {
                       Identity = entity.Identity,
                       UniqueName = entity.UniqueName,
                       Activated = entity.Activated
                   }));
        }

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
            }
        }

        public async ValueTask ProcessAsync(string businessObject, BusinessObjectInspectorEvent @event)
        {
            if (@event.NewInspector != null &&
                @event.BusinessObjectDisplayName != null)
            {
                var newInspector = await _inspectorManager.GetAsync(@event.NewInspector);

                newInspector.BusinessObjects.Add(new InspectorBusinessObject
                {
                    DisplayName = @event.BusinessObjectDisplayName,
                    UniqueName = businessObject,
                    AuditSchedulePlannedAuditDate = default,
                    AuditSchedulePlannedAuditTime = default,
                    AuditScheduleDelayed = false,
                    AuditSchedulePending = false
                });
            }

            if (@event.OldInspector != null)
            {
                var oldInspector = await _inspectorManager.GetAsync(@event.OldInspector);
                var oldBusinessObject = oldInspector.BusinessObjects.Single(x => x.UniqueName == businessObject);

                oldInspector.BusinessObjects.Remove(oldBusinessObject);

                await _inspectorManager.UpdateAsync(oldInspector);
            }
        }

        public async ValueTask ProcessAsync(string businessObject, BusinessObjectInspectionAuditScheduleEvent @event)
        {
            if (@event.Inspector == null)
            {
                return;
            }

            var inspector = await _inspectorManager.GetAsync(@event.Inspector);
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
            } 
            else
            {
                var newPlannedAuditTimestamp = (@event.PlannedAuditDate.Value, @event.PlannedAuditTime.Value)
                    .ToDateTime();

                var oldPlannedAuditTimestamp = (inspectorBusinessObject.AuditSchedulePlannedAuditDate, inspectorBusinessObject.AuditSchedulePlannedAuditTime)
                    .ToDateTime();

                var oldDelayed = inspectorBusinessObject.AuditScheduleDelayed;
                var oldPending = inspectorBusinessObject.AuditSchedulePending;

                inspectorBusinessObject.AuditSchedulePlannedAuditDate = @event.PlannedAuditDate.Value;
                inspectorBusinessObject.AuditSchedulePlannedAuditTime = @event.PlannedAuditTime.Value;

                var now = DateTime.UtcNow;
                var threshold = @event.Threshold ?? 0;

                inspectorBusinessObject.AuditScheduleDelayed = now > newPlannedAuditTimestamp.AddMilliseconds(threshold);
                inspectorBusinessObject.AuditSchedulePending = now > newPlannedAuditTimestamp.AddMilliseconds(-threshold);

                await PublishInspectorBusinessObjectAsync(inspector, inspectorBusinessObject, oldDelayed, oldPending);
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
                BusinessObject = inspectorBusinessObject.UniqueName,
                BusinessObjectDisplayName = inspectorBusinessObject.DisplayName,
                NewDelayed = inspectorBusinessObject.AuditScheduleDelayed,
                NewPending = inspectorBusinessObject.AuditSchedulePending,
                OldDelayed = oldDelayed,
                OldPending = pldPending
            };

            await _eventBus.PublishAsync(EventCategories.Notification, inspector.UniqueName, @event);
        }
    }
}