using Super.Paula.Aggregates.Auditing;
using Super.Paula.Data;
using Super.Paula.Environment;
using Super.Paula.Management;
using Super.Paula.Management.Auditing;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Server.Handling
{
    public class BusinessObjectInspectionAuditHandler : IBusinessObjectInspectionAuditHandler
    {
        private readonly IBusinessObjectInspectionAuditManager _businessObjectInspectionAuditManager;
        private readonly AppState _appState;

        public BusinessObjectInspectionAuditHandler(
            IBusinessObjectInspectionAuditManager businessObjectInspectionAuditManager,
            AppState appState)
        {
            _businessObjectInspectionAuditManager = businessObjectInspectionAuditManager;
            _appState = appState;
        }

        public async ValueTask<InspectionAuditResponse> GetAsync(string businessObject, string inspection, int date, int time)
        {
            var entity = await _businessObjectInspectionAuditManager.GetAsync(businessObject, inspection, date, time);

            return new InspectionAuditResponse
            {
                Annotation = entity.Annotation,
                AuditDate = entity.AuditDate,
                AuditTime = entity.AuditTime,
                BusinessObject = entity.BusinessObject,
                BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                Inspection = entity.Inspection,
                InspectionDisplayName = entity.InspectionDisplayName,
                Inspector = entity.Inspector ?? _appState.CurrentInspector,
                Result = entity.Result
            };
        }

        public IAsyncEnumerable<InspectionAuditResponse> GetAll()
            => _businessObjectInspectionAuditManager
                .GetAsyncEnumerable(query => query
                    .Select(entity => new InspectionAuditResponse
                    {
                        Annotation = entity.Annotation,
                        AuditDate = entity.AuditDate,
                        AuditTime = entity.AuditTime,
                        BusinessObject = entity.BusinessObject,
                        BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                        Inspection = entity.Inspection,
                        InspectionDisplayName = entity.InspectionDisplayName,
                        Inspector = entity.Inspector ?? string.Empty,
                        Result = entity.Result
                    }));

        public IAsyncEnumerable<InspectionAuditResponse> GetAllForBusinessObject(string businessObject)
            => _businessObjectInspectionAuditManager
                .GetAsyncEnumerable(query => query
                    .Where(entity => 
                        entity.BusinessObject == businessObject)
                    .Select(entity => new InspectionAuditResponse
                    {
                        Annotation = entity.Annotation,
                        AuditDate = entity.AuditDate,
                        AuditTime = entity.AuditTime,
                        BusinessObject = entity.BusinessObject,
                        BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                        Inspection = entity.Inspection,
                        InspectionDisplayName = entity.InspectionDisplayName,
                        Inspector = entity.Inspector ?? string.Empty,
                        Result = entity.Result
                    }));

        public async ValueTask<InspectionAuditResponse> CreateAsync(string businessObject, BusinessObjectInspectionAuditRequest request)
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

            return new InspectionAuditResponse
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

        public IAsyncEnumerable<InspectionAuditResponse> Search(string? businessObject, string? inspector, string? inspection)
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
                    .Select(entity => new InspectionAuditResponse
                    {
                        Annotation = entity.Annotation,
                        AuditDate = entity.AuditDate,
                        AuditTime = entity.AuditTime,
                        BusinessObject = entity.BusinessObject,
                        BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                        Inspection = entity.Inspection,
                        InspectionDisplayName = entity.InspectionDisplayName,
                        Inspector = entity.Inspector ?? string.Empty,
                        Result = entity.Result
                    }));

        }

        public IAsyncEnumerable<InspectionAuditResponse> SearchForBusinessObject(string businessObject, string? inspector, string? inspection)
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
                      .Select(entity => new InspectionAuditResponse
                      {
                          Annotation = entity.Annotation,
                          AuditDate = entity.AuditDate,
                          AuditTime = entity.AuditTime,
                          BusinessObject = entity.BusinessObject,
                          BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                          Inspection = entity.Inspection,
                          InspectionDisplayName = entity.InspectionDisplayName,
                          Inspector = entity.Inspector ?? string.Empty,
                          Result = entity.Result
                      }));
        }
    }
}