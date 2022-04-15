using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using Super.Paula.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionHandler : IBusinessObjectInspectionHandler
    {
        private readonly IBusinessObjectInspectionManager _businessObjectInspectionManager;
        private readonly IBusinessObjectInspectionEventService _businessObjectInspectionEventService;
        private readonly IBusinessObjectInspectionContinuationService _businessObjectInspectionContinuationService;
        private readonly IBusinessObjectInspectionAuditScheduler _businessObjectInspectionAuditScheduler;
        private readonly ClaimsPrincipal _user;

        public BusinessObjectInspectionHandler(
            IBusinessObjectInspectionManager businessObjectInspectionManager,
            IBusinessObjectInspectionEventService businessObjectInspectionEventService,
            IBusinessObjectInspectionContinuationService businessObjectInspectionContinuationService,
            IBusinessObjectInspectionAuditScheduler businessObjectInspectionAuditScheduler,
            ClaimsPrincipal user)
        {
            _businessObjectInspectionManager = businessObjectInspectionManager;
            _businessObjectInspectionEventService = businessObjectInspectionEventService;
            _businessObjectInspectionContinuationService = businessObjectInspectionContinuationService;
            _businessObjectInspectionAuditScheduler = businessObjectInspectionAuditScheduler;
            _user = user;
        }

        public async ValueTask<BusinessObjectInspectionResponse> GetAsync(string businessObject, string inspection)
        {
            var entity = await _businessObjectInspectionManager.GetAsync(businessObject, inspection);

            return new BusinessObjectInspectionResponse
            {
                Inspection = entity.Inspection,
                InspectionDisplayName = entity.InspectionDisplayName,
                InspectionText = entity.InspectionText,
                BusinessObject = entity.BusinessObject,
                BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                Activated = entity.Activated,
                AuditDate = entity.Audit.AuditDate,
                AuditTime = entity.Audit.AuditTime,
                AuditAnnotation = entity.Audit.Annotation,
                AuditInspector = entity.Audit.Inspector,
                AuditResult = entity.Audit.Result,
                AuditSchedule = entity.AuditSchedule.ToResponse(),
                ETag = entity.ETag
            };
        }

        public IAsyncEnumerable<BusinessObjectInspectionResponse> GetAllForBusinessObject(string businessObject)
            => _businessObjectInspectionManager
                .GetAsyncEnumerable(queryable => queryable
                    .Where(x => x.BusinessObject == businessObject)
                    .Select(entity => new BusinessObjectInspectionResponse
                    {
                        Inspection = entity.Inspection,
                        InspectionDisplayName = entity.InspectionDisplayName,
                        InspectionText = entity.InspectionText,
                        BusinessObject = entity.BusinessObject,
                        BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                        Activated = entity.Activated,
                        AuditDate = entity.Audit.AuditDate,
                        AuditTime = entity.Audit.AuditTime,
                        AuditAnnotation = entity.Audit.Annotation,
                        AuditInspector = entity.Audit.Inspector,
                        AuditResult = entity.Audit.Result,
                        AuditSchedule = entity.AuditSchedule.ToResponse(),
                        ETag = entity.ETag
                    }));

        public async ValueTask<BusinessObjectInspectionResponse> CreateAsync(string businessObject, BusinessObjectInspectionRequest request)
        {
            var (assignmentDate, assignmentTime) = DateTimeNumbers.GlobalNow;

            var entity = new BusinessObjectInspection
            {
                BusinessObject = businessObject,
                BusinessObjectDisplayName = request.BusinessObjectDisplayName,
                Inspection = request.Inspection,
                InspectionDisplayName = request.InspectionDisplayName,
                Activated = request.Activated,
                InspectionText = request.InspectionText,
                AssignmentDate = assignmentDate,
                AssignmentTime = assignmentTime,
                Audit = new BusinessObjectInspectionAudit(),
                AuditSchedule = new BusinessObjectInspectionAuditSchedule
                {
                    Threshold = TimeSpan.FromHours(8).Milliseconds,
                }
            };

            await _businessObjectInspectionManager.InsertAsync(entity);

            return new BusinessObjectInspectionResponse
            {
                Inspection = entity.Inspection,
                InspectionDisplayName = entity.InspectionDisplayName,
                InspectionText = entity.InspectionText,
                BusinessObject = entity.BusinessObject,
                BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                Activated = entity.Activated,
                AuditDate = entity.Audit.AuditDate,
                AuditTime = entity.Audit.AuditTime,
                AuditAnnotation = entity.Audit.Annotation,
                AuditInspector = entity.Audit.Inspector,
                AuditResult = entity.Audit.Result,
                AuditSchedule = entity.AuditSchedule.ToResponse(),
                ETag = entity.ETag
            };
        }

        public async ValueTask ReplaceAsync(string businessObject, string inspection, BusinessObjectInspectionRequest request)
        {
            var entity = await _businessObjectInspectionManager.GetAsync(businessObject, inspection);

            entity.BusinessObjectDisplayName = request.BusinessObjectDisplayName;
            entity.Inspection = request.Inspection;
            entity.InspectionDisplayName = request.InspectionDisplayName;
            entity.InspectionText = request.InspectionText;
            entity.Activated = request.Activated;
            entity.ETag = request.ETag;

            await _businessObjectInspectionManager.UpdateAsync(entity);
        }

        public async ValueTask DeleteAsync(string businessObject, string inspection, string etag)
        {
            var entity = await _businessObjectInspectionManager.GetAsync(businessObject, inspection);

            entity.ETag = etag;

            await _businessObjectInspectionManager.DeleteAsync(entity);

            var inspections = _businessObjectInspectionManager.GetQueryable()
                .Where(x => x.BusinessObject == businessObject)
                .ToList();

            await _businessObjectInspectionEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(inspections);
        }

        public async ValueTask<BusinessObjectInspectionAuditScheduleResponse> ReplaceAuditScheduleAsync(string businessObject, string inspection, BusinessObjectInspectionAuditScheduleRequest request)
        {
            var entity = await _businessObjectInspectionManager.GetAsync(businessObject, inspection);

            entity.ETag = request.ETag;

            entity.AuditSchedule.Threshold = request.Threshold;
            entity.AuditSchedule.Expressions.Clear();
            entity.AuditSchedule.Omissions.Clear();
            entity.AuditSchedule.Additionals.Clear();

            if (!string.IsNullOrWhiteSpace(request.Schedule))
            {
                var auditSchedule = new BusinessObjectInspectionAuditScheduleExpression
                {
                    CronExpression = request.Schedule
                };

                entity.AuditSchedule.Expressions.Add(auditSchedule);
            }

            _businessObjectInspectionAuditScheduler.Schedule(entity);
            await _businessObjectInspectionManager.UpdateAsync(entity);

            var inspections = _businessObjectInspectionManager.GetQueryable()
                .Where(x => x.BusinessObject == businessObject)
                .ToList();

            await _businessObjectInspectionEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(inspections);

            var response = entity.AuditSchedule.ToResponse();
            response.ETag = entity.ETag;
            return response;
        }

        public async ValueTask RecalculateInspectionAuditAppointmentsAsync(string businessObject)
        {
            var inspections = _businessObjectInspectionManager.GetQueryable()
                .Where(x => x.BusinessObject == businessObject)
                .ToList();

            foreach (var entity in inspections)
            {
                _businessObjectInspectionAuditScheduler.Schedule(entity);

                await _businessObjectInspectionManager.UpdateAsync(entity);
            }

            await _businessObjectInspectionEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(inspections);
        }

        public async ValueTask<BusinessObjectInspectionAuditOmissionResponse> CreateAuditOmissionAsync(string businessObject, string inspection, BusinessObjectInspectionAuditOmissionRequest request)
        {
            var entity = await _businessObjectInspectionManager.GetAsync(businessObject, inspection);

            var omission = new BusinessObjectInspectionAuditScheduleTimestamp
            {
                PlannedAuditDate = request.PlannedAuditDate,
                PlannedAuditTime = request.PlannedAuditTime,
            };

            entity.AuditSchedule.Omissions.Add(omission);

            _businessObjectInspectionAuditScheduler.Schedule(entity);
            await _businessObjectInspectionManager.UpdateAsync(entity);

            var inspections = _businessObjectInspectionManager.GetQueryable()
                .Where(x => x.BusinessObject == businessObject)
                .ToList();

            await _businessObjectInspectionEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(inspections);

            return new BusinessObjectInspectionAuditOmissionResponse
            {
                ETag = entity.ETag,
                Appointments = entity.AuditSchedule.Appointments.ToResponse()
            };
        }


        public async ValueTask<BusinessObjectInspectionAuditResponse> CreateAuditAsync(string businessObject, string inspection, BusinessObjectInspectionAuditRequest request)
        {
            var entity = await _businessObjectInspectionManager.GetAsync(businessObject, inspection);

            entity.Audit.AuditDate = request.RequestDate;
            entity.Audit.AuditTime = request.RequestTime;
            entity.Audit.Inspector = _user.GetInspector();
            entity.Audit.Annotation = string.Empty;
            entity.Audit.Result = request.Result;
            entity.ETag = request.ETag;

            _businessObjectInspectionAuditScheduler.Schedule(entity);
            await _businessObjectInspectionManager.UpdateAsync(entity);

            var inspections = _businessObjectInspectionManager.GetQueryable()
                .Where(x => x.BusinessObject == businessObject)
                .ToList();

            await _businessObjectInspectionContinuationService.AddCreateBusinessObjectInspectionAuditRecordContinuationAsync(entity);
            await _businessObjectInspectionEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(inspections);

            return new BusinessObjectInspectionAuditResponse
            {
                ETag = entity.ETag,
                Appointments = entity.AuditSchedule.Appointments.ToResponse()
            };
        }

        public async ValueTask<BusinessObjectInspectionAuditResponse> ReplaceAuditAsync(string businessObject, string inspection, BusinessObjectInspectionAuditRequest request)
        {
            var entity = await _businessObjectInspectionManager.GetAsync(businessObject, inspection);

            entity.ETag = request.ETag;

            entity.Audit.Inspector = _user.GetInspector();
            entity.Audit.Result = request.Result;

            await _businessObjectInspectionManager.UpdateAsync(entity);
            await _businessObjectInspectionContinuationService.AddCreateBusinessObjectInspectionAuditRecordContinuationAsync(entity);

            return new BusinessObjectInspectionAuditResponse
            {
                ETag = entity.ETag,
                Appointments = entity.AuditSchedule.Appointments.ToResponse()
            };
        }

        public async ValueTask<BusinessObjectInspectionAuditAnnotationResponse> ReplaceAuditAnnotationAsync(string businessObject, string inspection, BusinessObjectInspectionAuditAnnotationRequest request)
        {
            var entity = await _businessObjectInspectionManager.GetAsync(businessObject, inspection);

            entity.ETag = request.ETag;

            entity.Audit.Inspector = _user.GetInspector();
            entity.Audit.Annotation = request.Annotation;

            await _businessObjectInspectionManager.UpdateAsync(entity);
            await _businessObjectInspectionContinuationService.AddCreateBusinessObjectInspectionAuditRecordContinuationAsync(entity);

            return new BusinessObjectInspectionAuditAnnotationResponse
            {
                ETag = entity.ETag
            };
        }
    }
}