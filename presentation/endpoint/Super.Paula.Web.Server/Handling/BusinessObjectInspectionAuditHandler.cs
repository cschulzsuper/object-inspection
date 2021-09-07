using Super.Paula.Aggregates.Auditing;
using Super.Paula.Data;
using Super.Paula.Environment;
using Super.Paula.Management;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Server.Handling
{
    public class BusinessObjectInspectionAuditHandler : IBusinessObjectInspectionAuditHandler
    {
        private readonly IRepository<BusinessObjectInspectionAudit> _inspectionAuditRepository;
        private readonly AppState _appState;

        public BusinessObjectInspectionAuditHandler(
            IRepository<BusinessObjectInspectionAudit> inspectionAuditRepository,
            AppState appState)
        {
            _inspectionAuditRepository = inspectionAuditRepository;
            _appState = appState;
        }

        public async ValueTask<InspectionAuditResponse> GetAsync(string businessObject, string inspection, int date, int time)
        {
            var entity = await _inspectionAuditRepository.GetByIdsAsync(date, businessObject, inspection, time);

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
            => _inspectionAuditRepository
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

        public async ValueTask<InspectionAuditResponse> CreateAsync(InspectionAuditRequest request)
        {
            var entity = new BusinessObjectInspectionAudit
            {
                Annotation = request.Annotation,
                AuditDate = request.AuditDate,
                AuditTime = request.AuditTime,
                BusinessObject = request.BusinessObject,
                BusinessObjectDisplayName = request.BusinessObjectDisplayName,
                Inspection = request.Inspection,
                InspectionDisplayName = request.InspectionDisplayName,
                Inspector = request.Inspector,
                Result = request.Result
            };

            await _inspectionAuditRepository.InsertAsync(entity);

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

        public async ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, InspectionAuditRequest request)
        {
            var entity = await _inspectionAuditRepository.GetByIdsAsync(date, businessObject, inspection, time);

            entity.Annotation = request.Annotation;
            entity.AuditDate = request.AuditDate;
            entity.AuditTime = request.AuditTime;
            entity.BusinessObject = request.BusinessObject;
            entity.BusinessObjectDisplayName = request.BusinessObjectDisplayName;
            entity.Inspection = request.Inspection;
            entity.InspectionDisplayName = request.InspectionDisplayName;
            entity.Inspector = request.Inspector;
            entity.Result = request.Result;

            await _inspectionAuditRepository.UpdateAsync(entity);
        }

        public async ValueTask DeleteAsync(string businessObject, string inspection, int date, int time)
        {
            var entity = await _inspectionAuditRepository.GetByIdsAsync(date, businessObject, inspection, time);

            await _inspectionAuditRepository.DeleteAsync(entity);
        }

        public IAsyncEnumerable<InspectionAuditResponse> Search(string? businessObject, string? inspector, string? inspection)
        {
            var doBusinessObjectSearch = businessObject?.Length > 3;
            var doInspectorSearch = inspector?.Length > 3;
            var doInspectionSearch = inspection?.Length > 3;

            return _inspectionAuditRepository
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

            return _inspectionAuditRepository
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