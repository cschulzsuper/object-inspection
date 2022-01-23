using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using Super.Paula.Application.Inventory.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionAuditHandler : IBusinessObjectInspectionAuditHandler, IBusinessObjectInspectionAuditEventHandler
    {
        private readonly IBusinessObjectInspectionAuditManager _businessObjectInspectionAuditManager;

        public BusinessObjectInspectionAuditHandler(IBusinessObjectInspectionAuditManager businessObjectInspectionAuditManager)
        {
            _businessObjectInspectionAuditManager = businessObjectInspectionAuditManager;
        }

        public async ValueTask<BusinessObjectInspectionAuditResponse> GetAsync(string businessObject, string inspection, int date, int time)
        {
            var entity = await _businessObjectInspectionAuditManager.GetAsync(businessObject, inspection, date, time);

            return new BusinessObjectInspectionAuditResponse
            {
                Annotation = entity.Annotation,
                AuditDate = entity.AuditDate,
                AuditTime = entity.AuditTime,
                BusinessObject = entity.BusinessObject,
                BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                Inspection = entity.Inspection,
                InspectionDisplayName = entity.InspectionDisplayName,
                Inspector = entity.Inspector,
                Result = entity.Result
            };
        }

        public IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAll()
            => _businessObjectInspectionAuditManager
                .GetAsyncEnumerable(query => query
                    .Select(entity => new BusinessObjectInspectionAuditResponse
                    {
                        Annotation = entity.Annotation,
                        AuditDate = entity.AuditDate,
                        AuditTime = entity.AuditTime,
                        BusinessObject = entity.BusinessObject,
                        BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                        Inspection = entity.Inspection,
                        InspectionDisplayName = entity.InspectionDisplayName,
                        Inspector = entity.Inspector,
                        Result = entity.Result
                    }));

        public IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAllForBusinessObject(string businessObject)
            => _businessObjectInspectionAuditManager
                .GetAsyncEnumerable(query => query
                    .Where(entity => 
                        entity.BusinessObject == businessObject)
                    .Select(entity => new BusinessObjectInspectionAuditResponse
                    {
                        Annotation = entity.Annotation,
                        AuditDate = entity.AuditDate,
                        AuditTime = entity.AuditTime,
                        BusinessObject = entity.BusinessObject,
                        BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                        Inspection = entity.Inspection,
                        InspectionDisplayName = entity.InspectionDisplayName,
                        Inspector = entity.Inspector,
                        Result = entity.Result
                    }));

        public async ValueTask<BusinessObjectInspectionAuditResponse> CreateAsync(string businessObject, BusinessObjectInspectionAuditRequest request)
        {
            var entity = new BusinessObjectInspectionAudit
            {
                Annotation = request.Annotation,
                AuditDate = request.AuditDate,
                AuditTime = request.AuditTime,
                BusinessObject = businessObject,
                BusinessObjectDisplayName = request.BusinessObjectDisplayName,
                Inspection = request.Inspection,
                InspectionDisplayName = request.InspectionDisplayName,
                Inspector = request.Inspector,
                Result = request.Result
            };

            await _businessObjectInspectionAuditManager.InsertAsync(entity);

            return new BusinessObjectInspectionAuditResponse
            {
                Annotation = entity.Annotation,
                AuditDate = entity.AuditDate,
                AuditTime = entity.AuditTime,
                BusinessObject = entity.BusinessObject,
                BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                Inspection = entity.Inspection,
                InspectionDisplayName = entity.InspectionDisplayName,
                Inspector = entity.Inspector,
                Result = entity.Result
            };
        }

        public async ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRequest request)
        {
            var entity = await _businessObjectInspectionAuditManager.GetAsync(businessObject, inspection, date, time);

            entity.Annotation = request.Annotation;
            entity.AuditDate = request.AuditDate;
            entity.AuditTime = request.AuditTime;
            entity.BusinessObject = businessObject;
            entity.BusinessObjectDisplayName = request.BusinessObjectDisplayName;
            entity.Inspection = request.Inspection;
            entity.InspectionDisplayName = request.InspectionDisplayName;
            entity.Inspector = request.Inspector;
            entity.Result = request.Result;

            await _businessObjectInspectionAuditManager.UpdateAsync(entity);
        }

        public async ValueTask DeleteAsync(string businessObject, string inspection, int date, int time)
        {
            var entity = await _businessObjectInspectionAuditManager.GetAsync(businessObject, inspection, date, time);

            await _businessObjectInspectionAuditManager.DeleteAsync(entity);
        }

        public IAsyncEnumerable<BusinessObjectInspectionAuditResponse> Search(string? businessObject, string? inspector, string? inspection)
        {
            var doBusinessObjectSearch = businessObject?.Length > 3;
            var doInspectorSearch = inspector?.Length > 3;
            var doInspectionSearch = inspection?.Length > 3;

            return _businessObjectInspectionAuditManager
                .GetAsyncEnumerable(query => query
                    .Where(x => !doBusinessObjectSearch ||
                        (x.BusinessObject.Contains(businessObject!) || x.BusinessObjectDisplayName.Contains(businessObject!)))
                    .Where(x => !doInspectionSearch ||
                        (x.Inspection.Contains(inspection!) || x.InspectionDisplayName.Contains(inspection!)))
                    .Where(x => !doInspectorSearch ||
                        (x.Inspector.Contains(inspector!)))
                    .Select(entity => new BusinessObjectInspectionAuditResponse
                    {
                        Annotation = entity.Annotation,
                        AuditDate = entity.AuditDate,
                        AuditTime = entity.AuditTime,
                        BusinessObject = entity.BusinessObject,
                        BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                        Inspection = entity.Inspection,
                        InspectionDisplayName = entity.InspectionDisplayName,
                        Inspector = entity.Inspector,
                        Result = entity.Result
                    }));

        }

        public IAsyncEnumerable<BusinessObjectInspectionAuditResponse> SearchForBusinessObject(string businessObject, string? inspector, string? inspection)
        {
            var doInspectorSearch = inspector?.Length > 3;
            var doInspectionSearch = inspection?.Length > 3;

            return _businessObjectInspectionAuditManager
                  .GetAsyncEnumerable(query => query
                      .Where(entity =>
                          entity.BusinessObject == businessObject)
                      .Where(x => !doInspectionSearch ||
                          (x.Inspection.Contains(inspection!) || x.InspectionDisplayName.Contains(inspection!)))
                      .Where(x => !doInspectorSearch ||
                          (x.Inspector.Contains(inspector!)))
                      .Select(entity => new BusinessObjectInspectionAuditResponse
                      {
                          Annotation = entity.Annotation,
                          AuditDate = entity.AuditDate,
                          AuditTime = entity.AuditTime,
                          BusinessObject = entity.BusinessObject,
                          BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                          Inspection = entity.Inspection,
                          InspectionDisplayName = entity.InspectionDisplayName,
                          Inspector = entity.Inspector,
                          Result = entity.Result
                      }));
        }

        public async ValueTask ProcessAsync(string businessObject, BusinessObjectEvent @event)
        {
            var businessObjectInspectionAudits = _businessObjectInspectionAuditManager
                .GetQueryable()
                .Where(entity => entity.BusinessObject == businessObject);

            if (@event.DisplayName != null)
            {
                businessObjectInspectionAudits = businessObjectInspectionAudits
                    .Where(entity => entity.BusinessObjectDisplayName != @event.DisplayName);
            }

            foreach (var businessObjectInspectionAudit in businessObjectInspectionAudits)
            {
                businessObjectInspectionAudit.BusinessObjectDisplayName = @event.DisplayName ?? businessObjectInspectionAudit.BusinessObjectDisplayName;

                await _businessObjectInspectionAuditManager.UpdateAsync(businessObjectInspectionAudit);
            }
        }

        public async ValueTask ProcessAsync(string businessObject, BusinessObjectInspectionAuditEvent @event)
        {
            var businessObjectInspectionAudit = await _businessObjectInspectionAuditManager.GetOrDefaultAsync(
                    businessObject, 
                    @event.Inspection, 
                    @event.AuditDate,
                    @event.AuditTime);

            if (businessObjectInspectionAudit != null)
            {
                businessObjectInspectionAudit.InspectionDisplayName = @event.InspectionDisplayName ?? businessObjectInspectionAudit.InspectionDisplayName;
                businessObjectInspectionAudit.Annotation = @event.AuditAnnotation ?? businessObjectInspectionAudit.Annotation;
                businessObjectInspectionAudit.Result = @event.AuditResult ?? businessObjectInspectionAudit.Result;
                businessObjectInspectionAudit.BusinessObjectDisplayName = @event.BusinessObjectDisplayName ?? businessObjectInspectionAudit.BusinessObjectDisplayName;
                businessObjectInspectionAudit.Inspector = @event.AuditInspector;

                await _businessObjectInspectionAuditManager.UpdateAsync(businessObjectInspectionAudit);
            }
            else
            {
                businessObjectInspectionAudit = new BusinessObjectInspectionAudit
                {
                    Annotation = @event.AuditAnnotation ?? string.Empty,
                    AuditDate = @event.AuditDate,
                    AuditTime = @event.AuditTime,
                    BusinessObject = businessObject,
                    BusinessObjectDisplayName = @event.BusinessObjectDisplayName ?? string.Empty,
                    Inspection = @event.Inspection,
                    InspectionDisplayName = @event.InspectionDisplayName ?? string.Empty,
                    Inspector = @event.AuditInspector,
                    Result = @event.AuditResult ?? string.Empty
                };

                await _businessObjectInspectionAuditManager.InsertAsync(businessObjectInspectionAudit);
            }
        }
    }
}