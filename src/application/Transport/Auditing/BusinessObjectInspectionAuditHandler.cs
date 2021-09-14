using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using Super.Paula.Environment;

namespace Super.Paula.Application.Auditing
{
    internal class BusinessObjectInspectionAuditHandler : IBusinessObjectInspectionAuditHandler
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
                Inspector = entity.Inspector ?? _appState.CurrentInspector,
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
                        Inspector = entity.Inspector ?? string.Empty,
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
                        Inspector = entity.Inspector ?? string.Empty,
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
                        Inspector = entity.Inspector ?? string.Empty,
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
                          Inspector = entity.Inspector ?? string.Empty,
                          Result = entity.Result
                      }));
        }
    }
}