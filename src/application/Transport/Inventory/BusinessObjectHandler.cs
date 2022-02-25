using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;
using Super.Paula.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectHandler : IBusinessObjectHandler
    {
        private const string SearchTermKeyFreeText = "";
        private const string SearchTermKeyBusinessObject = "business-object";
        private const string SearchTermKeyInspector = "inspector";

        private readonly IBusinessObjectManager _businessObjectManager;
        private readonly IInspectionManager _inspectionManager;
        private readonly ClaimsPrincipal _user;
        private readonly IBusinessObjectEventService _businessObjectEventService;
        private readonly IBusinessObjectInspectionAuditScheduleFilter _businessObjectInspectionAuditScheduleFilter;

        public BusinessObjectHandler(
            IBusinessObjectManager businessObjectManager,
            IInspectionManager inspectionManager,
            ClaimsPrincipal user,
            IBusinessObjectEventService businessObjectEventService,
            IBusinessObjectInspectionAuditScheduleFilter businessObjectInspectionAuditScheduleFilter)
        {
            _businessObjectManager = businessObjectManager;
            _inspectionManager = inspectionManager;
            _user = user;
            _businessObjectEventService = businessObjectEventService;
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

        public IAsyncEnumerable<BusinessObjectResponse> GetAll(string query, int skip, int take, CancellationToken cancellationToken = default)
            => _businessObjectManager
                .GetAsyncEnumerable(queryable => WhereSearchQuery(queryable, query)
                    .Skip(skip)
                    .Take(take)
                    .Select(entity => new BusinessObjectResponse
                    {
                        Inspections = entity.Inspections.ToResponse(),
                        Inspector = entity.Inspector,
                        DisplayName = entity.DisplayName,
                        UniqueName = entity.UniqueName
                    }));

        public async ValueTask<SearchBusinessObjectResponse> SearchAsync(string query)
        {
            await ValueTask.CompletedTask;

            var queryable = _businessObjectManager.GetQueryable();
            queryable = WhereSearchQuery(queryable, query);

            var topResult = queryable.Take(50)
                .Select(entity => new BusinessObjectResponse
                {
                    Inspections = entity.Inspections.ToResponse(),
                    Inspector = entity.Inspector,
                    DisplayName = entity.DisplayName,
                    UniqueName = entity.UniqueName
                })
                .ToHashSet();

            return new SearchBusinessObjectResponse
            {
                TotalCount = queryable.Count(),
                TopResults = topResult
            };
        }

        public async ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request)
        {
            var entity = new BusinessObject
            {
                Inspector = request.Inspector,
                DisplayName = request.DisplayName,
                UniqueName = request.UniqueName
            };

            await _businessObjectManager.InsertAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectEventAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectInspectorEventAsync(entity, entity.Inspector, string.Empty);

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
                await _businessObjectEventService.CreateBusinessObjectEventAsync(entity);
                await _businessObjectEventService.CreateBusinessObjectInspectorEventAsync(entity, entity.Inspector, oldInspector);
            }
        }

        public async ValueTask DeleteAsync(string businessObject)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            await _businessObjectManager.DeleteAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectDeletionEventAsync(businessObject);
            await _businessObjectEventService.CreateBusinessObjectInspectorEventAsync(entity, string.Empty, entity.Inspector);
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
            await _businessObjectEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(entity);
        }

        public async ValueTask TimeInspectionAuditAsync(string businessObject)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            foreach (var inspection in entity.Inspections)
            {
                _businessObjectInspectionAuditScheduleFilter.Apply(
                    new BusinessObjectInspectionAuditScheduleFilterContext(
                        Inspection: inspection,
                        Limit: DateTime.UtcNow.AddMonths(1).ToNumbers()));
            }

            await _businessObjectManager.UpdateAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(entity);
        }

        public async ValueTask AssignInspectionAsync(string businessObject, AssignInspectionRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);
            var inspection = await _inspectionManager.GetAsync(request.UniqueName);

            var (assignmentDate, assignmentTime) = DateTime.UtcNow.ToNumbers();

            entity.Inspections.Add(new BusinessObjectInspection
            {
                Activated = inspection.Activated,

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
            inspection.AuditInspector = _user.GetInspector();
            inspection.AuditAnnotation = string.Empty;
            inspection.AuditResult = request.Result;

            _businessObjectInspectionAuditScheduleFilter.Apply(
                new BusinessObjectInspectionAuditScheduleFilterContext(
                    Inspection: inspection,
                    Limit: DateTime.UtcNow.AddMonths(1).ToNumbers()));

            await _businessObjectManager.UpdateAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectInspectionAuditEventAsync(entity, inspection);
            await _businessObjectEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(entity);

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

            businessObjectInspection.AuditInspector = _user.GetInspector();
            businessObjectInspection.AuditResult = request.Result;

            await _businessObjectManager.UpdateAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectInspectionAuditEventAsync(entity, businessObjectInspection);
        }

        public async ValueTask AnnotateInspectionAuditAsync(string businessObject, string inspection, AnnotateInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var businessObjectInspection = entity.Inspections
                .Single(x => x.UniqueName == inspection);

            businessObjectInspection.AuditInspector = _user.GetInspector();
            businessObjectInspection.AuditAnnotation = request.Annotation;

            await _businessObjectManager.UpdateAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectInspectionAuditEventAsync(entity, businessObjectInspection);
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
            await _businessObjectEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(entity);

            return new DropInspectionAuditResponse
            {
                BusinessObject = businessObject,
                Inspection = inspection,
                Appointments = businessObjectInspection.AuditSchedule.Appointments.ToResponse()
            };
        }

        private static IQueryable<BusinessObject> WhereSearchQuery(IQueryable<BusinessObject> query, string searchQuery)
        {
            var searchTerms = SearchQueryParser.Parse(searchQuery);
            var businessObjects = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyBusinessObject);
            var inspectors = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyInspector);
            var freeTexts = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyFreeText).Where(x => x.Length > 3).ToArray();

            query = query
                 .Where(x => !businessObjects.Any() || businessObjects.Contains(x.UniqueName))
                 .Where(x => !inspectors.Any() || inspectors.Contains(x.Inspector));

            foreach (var freeText in freeTexts)
            {
                query = query.Where(x => x.DisplayName.Contains(freeText));
            }

            return query.OrderByDescending(x => x.UniqueName);
        }
    }
}