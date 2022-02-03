using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;
using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Administration.Responses;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectHandler : IBusinessObjectHandler, IBusinessObjectEventHandler
    {
        private readonly IBusinessObjectManager _businessObjectManager;
        private readonly IInspectionProvider _inspectionProvider;
        private readonly AppState _appState;
        private readonly IEventBus _eventBus;
        private readonly IBusinessObjectInspectionAuditScheduleFilter _businessObjectInspectionAuditScheduleFilter;

        public BusinessObjectHandler(
            IBusinessObjectManager businessObjectManager,
            IInspectionProvider inspectionProvider,
            AppState appState,
            IEventBus eventBus,
            IBusinessObjectInspectionAuditScheduleFilter businessObjectInspectionAuditScheduleFilter)
        {
            _businessObjectManager = businessObjectManager;
            _inspectionProvider = inspectionProvider;
            _appState = appState;
            _eventBus = eventBus;
            _businessObjectInspectionAuditScheduleFilter = businessObjectInspectionAuditScheduleFilter;
        }

        public async ValueTask<BusinessObjectResponse> GetAsync(string businessObject)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            return new BusinessObjectResponse
            {
                Inspections = entity.Inspections.ToResponse(),
                Inspector = entity.Inspector,
                DisplayName = entity.DisplayName,
                UniqueName = entity.UniqueName
            };
        }

        public IAsyncEnumerable<BusinessObjectResponse> GetAll()
            => _businessObjectManager
                .GetAsyncEnumerable(query => query
                .Select(entity => new BusinessObjectResponse
                {
                    Inspections = entity.Inspections.ToResponse(),
                    Inspector = entity.Inspector,
                    DisplayName = entity.DisplayName,
                    UniqueName = entity.UniqueName
                }));

        public async ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request)
        {
            var entity = new BusinessObject
            {
                Inspector = request.Inspector,
                DisplayName = request.DisplayName,
                UniqueName = request.UniqueName
            };

            await _businessObjectManager.InsertAsync(entity);

            await PublishBusinessObjectInspectorAsync(entity);

            return new BusinessObjectResponse
            {
                Inspections = entity.Inspections.ToResponse(),
                Inspector = entity.Inspector,
                DisplayName = entity.DisplayName,
                UniqueName = entity.UniqueName
            };
        }

        public async ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var oldInspector = entity.Inspector;

            var required =
                entity.DisplayName != request.DisplayName ||
                entity.Inspector != request.Inspector;

            if (required)
            {
                entity.Inspector = request.Inspector;
                entity.DisplayName = request.DisplayName;
                entity.UniqueName = request.UniqueName;

                await _businessObjectManager.UpdateAsync(entity);

                await PublishBusinessObjectAsync(entity);
                await PublishBusinessObjectInspectorAsync(entity, oldInspector);
            }
        }

        public async ValueTask DeleteAsync(string businessObject)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            await _businessObjectManager.DeleteAsync(entity);
        }

        public async ValueTask ScheduleInspectionAuditAsync(string businessObject, string inspection, ScheduleInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var businessObjectInspection = entity.Inspections
                .Single(x => x.UniqueName == inspection);

            businessObjectInspection.AuditSchedule.Threshold = request.Threshold;

            businessObjectInspection.AuditSchedule.Expressions.Clear();
            businessObjectInspection.AuditSchedule.Omissions.Clear();
            businessObjectInspection.AuditSchedule.Additionals.Clear();

            if (!string.IsNullOrWhiteSpace(request.Schedule))
            {
                var auditSchedule = new BusinessObjectInspectionAuditScheduleExpression
                {
                    CronExpression = request.Schedule
                };

                businessObjectInspection.AuditSchedule.Expressions.Add(auditSchedule);
            }

            _businessObjectInspectionAuditScheduleFilter.Apply(
                new BusinessObjectInspectionAuditScheduleFilterContext(
                    Inspection: businessObjectInspection,
                    Limit: DateTime.UtcNow.AddMonths(1).ToNumbers()));

            await _businessObjectManager.UpdateAsync(entity);

            await PublishBusinessObjectInspectionAuditScheduleEventAsync(entity);
        }

        public async ValueTask TimeInspectionAuditAsync(string businessObject)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            foreach(var inspection in entity.Inspections)
            {
                _businessObjectInspectionAuditScheduleFilter.Apply(
                    new BusinessObjectInspectionAuditScheduleFilterContext(
                        Inspection: inspection,
                        Limit: DateTime.UtcNow.AddMonths(1).ToNumbers()));
            }

            await _businessObjectManager.UpdateAsync(entity);

            await PublishBusinessObjectInspectionAuditScheduleEventAsync(entity);
        }

        public async ValueTask AssignInspectionAsync(string businessObject, AssignInspectionRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);
            var inspection = await _inspectionProvider.GetAsync(request.UniqueName);

            var (assignmentDate, assignmentTime) = DateTime.UtcNow.ToNumbers();

            entity.Inspections.Add(new BusinessObjectInspection
            {
                Activated = true,
                ActivatedGlobally = inspection.Activated,

                UniqueName = inspection.UniqueName,
                DisplayName = inspection.DisplayName,
                Text = inspection.Text,

                AssignmentDate = assignmentDate,
                AssignmentTime = assignmentTime,

                AuditAnnotation = string.Empty,
                AuditInspector = string.Empty,
                AuditResult = string.Empty,
                AuditDate = default,
                AuditTime = default,

                AuditSchedule = new BusinessObjectInspectionAuditSchedule
                {
                    Expressions = new HashSet<BusinessObjectInspectionAuditScheduleExpression>
                    {
                        new BusinessObjectInspectionAuditScheduleExpression()
                    },
                    Threshold = TimeSpan.FromHours(8).Milliseconds,
                }
            });

            await _businessObjectManager.UpdateAsync(entity);
        }

        public async ValueTask CancelInspectionAsync(string businessObject, CancelInspectionRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var inspection = entity.Inspections
                .Single(inspection => inspection.UniqueName == request.UniqueName);

            entity.Inspections.Remove(inspection);

            await _businessObjectManager.UpdateAsync(entity);
        }

        public async ValueTask<CreateInspectionAuditResponse> CreateInspectionAuditAsync(string businessObject, CreateInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var inspection = entity.Inspections
                .Single(inspection => inspection.UniqueName == request.Inspection);

            inspection.AuditDate = request.AuditDate;
            inspection.AuditTime = request.AuditTime;
            inspection.AuditInspector = _appState.CurrentInspector;
            inspection.AuditAnnotation = string.Empty;
            inspection.AuditResult = request.Result;

            _businessObjectInspectionAuditScheduleFilter.Apply(
                new BusinessObjectInspectionAuditScheduleFilterContext(
                    Inspection: inspection, 
                    Limit: DateTime.UtcNow.AddMonths(1).ToNumbers()));

            await _businessObjectManager.UpdateAsync(entity);

            await PublishBusinessObjectInspectionAuditAsync(entity, inspection);
            await PublishBusinessObjectInspectionAuditScheduleEventAsync(entity);

            return new CreateInspectionAuditResponse
            {
                BusinessObject = businessObject,
                Inspection = inspection.UniqueName,
                Appointments = inspection.AuditSchedule.Appointments.ToResponse()
            };
        }

        public async ValueTask ChangeInspectionAuditAsync(string businessObject, string inspection, ChangeInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var businessObjectInspection = entity.Inspections
                .Single(x => x.UniqueName == inspection);

            businessObjectInspection.AuditInspector = _appState.CurrentInspector;
            businessObjectInspection.AuditResult = request.Result;

            await _businessObjectManager.UpdateAsync(entity);

            await PublishBusinessObjectInspectionAuditAsync(entity, businessObjectInspection);
        }

        public async ValueTask AnnotateInspectionAuditAsync(string businessObject, string inspection, AnnotateInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var businessObjectInspection = entity.Inspections
                .Single(x => x.UniqueName == inspection);

            businessObjectInspection.AuditInspector = _appState.CurrentInspector;
            businessObjectInspection.AuditAnnotation = request.Annotation;

            await _businessObjectManager.UpdateAsync(entity);

            await PublishBusinessObjectInspectionAuditAsync(entity, businessObjectInspection);
        }

        public IAsyncEnumerable<BusinessObjectResponse> Search(string? businessObject, string? inspector)
        {
            var doBusinessObjectSearch = businessObject?.Length > 3;
            var doInspectorSearch = inspector?.Length > 3;

            return _businessObjectManager
                .GetAsyncEnumerable(query => query

                    .Where(x => !doBusinessObjectSearch || 
                        (x.UniqueName.Contains(businessObject!) || x.DisplayName.Contains(businessObject!)))
                    .Where(x => !doInspectorSearch || 
                        (x.Inspector.Contains(inspector!)))

                    .Select(entity => new BusinessObjectResponse
                    {
                        Inspections = entity.Inspections.ToResponse(),
                        Inspector = entity.Inspector,
                        DisplayName = entity.DisplayName,
                        UniqueName = entity.UniqueName
                    }));
        }

        public IAsyncEnumerable<BusinessObjectResponse> GetAllForInspector(string inspector)
            => _businessObjectManager
                .GetAsyncEnumerable(query => query
                .Where(x => x.Inspector == inspector)
                .Select(entity => new BusinessObjectResponse
                {
                    Inspections = entity.Inspections.ToResponse(),
                    Inspector = entity.Inspector,
                    DisplayName = entity.DisplayName,
                    UniqueName = entity.UniqueName
                }));

        public async ValueTask ProcessAsync(string inspection, InspectionEvent @event)
        {
            var businessObjects = _businessObjectManager.GetQueryableWhereInspection(inspection)
                .AsEnumerable();

            foreach (var businessObject in businessObjects)
            {
                foreach(var businessObjectInspection in businessObject.Inspections)
                {
                    if (businessObjectInspection.UniqueName == inspection)
                    {
                        businessObjectInspection.DisplayName = @event.DisplayName ?? businessObjectInspection.DisplayName;
                        businessObjectInspection.Text = @event.Text ?? businessObjectInspection.Text;
                        businessObjectInspection.ActivatedGlobally = @event.Activated ?? businessObjectInspection.ActivatedGlobally;
                    }
                }

                await _businessObjectManager.UpdateAsync(businessObject);

                foreach (var businessObjectInspection in businessObject.Inspections)
                {
                    if (businessObjectInspection.UniqueName == inspection)
                    {
                        await PublishBusinessObjectInspectionAuditAsync(businessObject, businessObjectInspection);
                    }
                }
            }
        }

        private async ValueTask PublishBusinessObjectAsync(BusinessObject businessObject)
        {
            var @event = new BusinessObjectEvent
            {
                DisplayName = businessObject.DisplayName,
            };

            await _eventBus.PublishAsync(EventCategories.BusinessObjectInspectionAudit, businessObject.UniqueName, @event);
        }

        private async ValueTask PublishBusinessObjectInspectorAsync(BusinessObject businessObject, string? oldInspector = null)
        {
            if (businessObject.Inspector == oldInspector)
            {
                return;
            }

            var @event = new BusinessObjectInspectorEvent
            {
                BusinessObjectDisplayName = businessObject.DisplayName,
                NewInspector = businessObject.Inspector,
                OldInspector = oldInspector
            };

            await _eventBus.PublishAsync(EventCategories.Notification, businessObject.UniqueName, @event);
            await _eventBus.PublishAsync(EventCategories.Inspector, businessObject.UniqueName, @event);
        }

        private async ValueTask PublishBusinessObjectInspectionAuditAsync(BusinessObject businessObject, BusinessObjectInspection inspection)
        {
            if (inspection.AuditDate == default ||
                inspection.AuditTime == default)
            {
                return;
            }

            var @event = new BusinessObjectInspectionAuditEvent
            {
                BusinessObjectDisplayName = businessObject.DisplayName,
                Inspection = inspection.UniqueName,
                InspectionDisplayName = inspection.DisplayName,
                AuditInspector = inspection.AuditInspector,
                AuditDate = inspection.AuditDate,
                AuditTime = inspection.AuditTime,
                AuditResult = inspection.AuditResult,
                AuditAnnotation = inspection.AuditAnnotation,
            };

            await _eventBus.PublishAsync(EventCategories.BusinessObjectInspectionAudit, businessObject.UniqueName, @event);
        }

        private async ValueTask PublishBusinessObjectInspectionAuditScheduleEventAsync(BusinessObject businessObject)
        {
            var inspection = businessObject.Inspections
                .Where(x => x.AuditSchedule.Appointments.Any())
                .OrderBy(x => x.AuditSchedule.Appointments
                    .Min(y => (y.PlannedAuditDate, y.PlannedAuditTime).ToDateTime()))
                .FirstOrDefault();

            if(inspection == null)
            {
                return;
            }

            var @event = new BusinessObjectInspectionAuditScheduleEvent
            {
                Inspector = businessObject.Inspector,
                Threshold = inspection.AuditSchedule.Threshold,
                PlannedAuditDate = inspection.AuditSchedule.Appointments.First().PlannedAuditDate,
                PlannedAuditTime = inspection.AuditSchedule.Appointments.First().PlannedAuditTime
            };

            await _eventBus.PublishAsync(EventCategories.Inspector, businessObject.UniqueName, @event);
        }

        public async ValueTask<DropInspectionAuditResponse> DropInspectionAuditAsync(string businessObject, string inspection, DropInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var businessObjectInspection = entity.Inspections
                .Single(x => x.UniqueName == inspection);

            var omission = new BusinessObjectInspectionAuditScheduleTimestamp
            {
                PlannedAuditDate = request.PlannedAuditDate,
                PlannedAuditTime = request.PlannedAuditTime,
            };

            businessObjectInspection.AuditSchedule.Omissions.Add(omission);

            _businessObjectInspectionAuditScheduleFilter.Apply(
                new BusinessObjectInspectionAuditScheduleFilterContext(
                    Inspection: businessObjectInspection,
                    Limit: DateTime.UtcNow.AddMonths(1).ToNumbers()));

            await _businessObjectManager.UpdateAsync(entity);

            await PublishBusinessObjectInspectionAuditScheduleEventAsync(entity);

            return new DropInspectionAuditResponse
            {
                BusinessObject = businessObject,
                Inspection = inspection,
                Appointments = businessObjectInspection.AuditSchedule.Appointments.ToResponse()
            };
        }
    }
}