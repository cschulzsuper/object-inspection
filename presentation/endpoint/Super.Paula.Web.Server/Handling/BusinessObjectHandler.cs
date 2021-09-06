using Super.Paula.Aggregates.BusinessObjects;
using Super.Paula.Environment;
using Super.Paula.Management.Contract;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Server.Handling
{
    public class BusinessObjectHandler : IBusinessObjectHandler
    {
        private readonly IBusinessObjectManager _businessObjectManager;
        private readonly Lazy<IInspectionHandler> _inspectionHandler;
        private readonly IBusinessObjectInspectionAuditHandler _businessObjectInspectionAuditHandler;
        private readonly IInspectionBusinessObjectHandler _inspectionBusinessObjectHandler;
        private readonly AppState _appState;

        public BusinessObjectHandler(
            IBusinessObjectManager businessObjectManager,
            Lazy<IInspectionHandler> inspectionHandler,
            IBusinessObjectInspectionAuditHandler businessObjectInspectionAuditHandler,
            IInspectionBusinessObjectHandler inspectionBusinessObjectHandler,
            AppState appState)
        {
            _businessObjectManager = businessObjectManager;
            _inspectionHandler = inspectionHandler;
            _businessObjectInspectionAuditHandler = businessObjectInspectionAuditHandler;
            _inspectionBusinessObjectHandler = inspectionBusinessObjectHandler;
            _appState = appState;
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

            return new BusinessObjectResponse
            {
                Inspections = entity.Inspections.ToEmbeddedResponses(),
                Inspector = entity.Inspector,
                DisplayName = entity.DisplayName,
                UniqueName = entity.UniqueName
            };
        }

        public async ValueTask DeleteAsync(string businessObject)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            await _businessObjectManager.DeleteAsync(entity);
        }

        public IAsyncEnumerable<BusinessObjectResponse> GetAll()
            => _businessObjectManager
            .GetAsyncEnumerable(query => query
                .Select(entity => new BusinessObjectResponse
                {
                    Inspections = entity.Inspections.ToEmbeddedResponses(),
                    Inspector = entity.Inspector,
                    DisplayName = entity.DisplayName,
                    UniqueName = entity.UniqueName
                }));
            

        public async ValueTask<BusinessObjectResponse> GetAsync(string businessObject)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            return new BusinessObjectResponse
            {
                Inspections = entity.Inspections.ToEmbeddedResponses(),
                Inspector = entity.Inspector,
                DisplayName = entity.DisplayName,
                UniqueName = entity.UniqueName
            };
        }

        public async ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var refresh =
                entity.DisplayName != request.DisplayName ||
                entity.UniqueName != request.UniqueName;

            entity.Inspector = request.Inspector;
            entity.DisplayName = request.DisplayName;
            entity.UniqueName = request.UniqueName;

            await _businessObjectManager.UpdateAsync(entity);

            if (refresh)
            {
                foreach (var bussinesObjectInspection in entity.Inspections)
                {
                    await _businessObjectInspectionAuditHandler.ReplaceAsync(
                        entity.UniqueName,
                        bussinesObjectInspection.Inspection,
                        bussinesObjectInspection.AuditDate,
                        bussinesObjectInspection.AuditTime,
                        new InspectionAuditRequest
                        {
                            Annotation = bussinesObjectInspection.AuditAnnotation,
                            AuditDate = bussinesObjectInspection.AuditDate,
                            AuditTime = bussinesObjectInspection.AuditTime,
                            BusinessObject = entity.UniqueName,
                            BusinessObjectDisplayName = entity.DisplayName,
                            Inspection = bussinesObjectInspection.Inspection,
                            InspectionDisplayName = bussinesObjectInspection.InspectionDisplayName,
                            Inspector = bussinesObjectInspection.AuditInspector,
                            Result = bussinesObjectInspection.AuditResult
                        });
                }
            }

        }

        public async ValueTask AssignInspectionAsync(string businessObject, AssignInspectionRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);
            var inspection = await _inspectionHandler.Value.GetAsync(request.UniqueName);

            entity.Inspections.Add(new BusinessObjectInspection
            {
                Activated = true,
                InspectionActivated = inspection.Activated,

                Inspection = inspection.UniqueName,
                InspectionDisplayName = inspection.DisplayName,
                InspectionText = inspection.Text,
            });

            await _businessObjectManager.UpdateAsync(entity);

            await _inspectionBusinessObjectHandler.CreateAsync(
                new InspectionBusinessObjectRequest
                {
                    BusinessObject = businessObject,

                    Inspection = inspection.UniqueName,
                    InspectionActivated = inspection.Activated,
                    InspectionDisplayName = inspection.DisplayName,
                    InspectionText = inspection.Text,
                });
        }

        public async ValueTask CancelInspectionAsync(string businessObject, CancelInspectionRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var inspection = entity.Inspections
                .Single(inspection => inspection.Inspection == request.UniqueName);

            entity.Inspections.Remove(inspection);

            await _businessObjectManager.UpdateAsync(entity);

            await _inspectionBusinessObjectHandler.DeleteAsync(request.UniqueName, businessObject);
        }

        public async ValueTask CreateInspectionAuditAsync(string businessObject, CreateInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var inspection = entity.Inspections
                .Single(inspection => inspection.Inspection == request.Inspection);

            inspection.AuditDate = request.AuditDate;
            inspection.AuditTime = request.AuditTime;
            inspection.AuditInspector = _appState.CurrentInspector;
            inspection.AuditAnnotation = string.Empty;
            inspection.AuditResult = request.Result;

            await _businessObjectManager.UpdateAsync(entity);

            await _businessObjectInspectionAuditHandler.CreateAsync(
                new InspectionAuditRequest
                {
                    Annotation = inspection.AuditAnnotation,
                    AuditDate = inspection.AuditDate,
                    AuditTime = inspection.AuditTime,
                    BusinessObject = entity.UniqueName,
                    BusinessObjectDisplayName = entity.DisplayName,
                    Inspection = inspection.Inspection,
                    InspectionDisplayName = inspection.InspectionDisplayName,
                    Inspector = inspection.AuditInspector,
                    Result = inspection.AuditResult
                });
        }

        public async ValueTask ChangeInspectionAuditAsync(string businessObject, string inspection, ChangeInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var inspectionEntity = entity.Inspections
                .Single(x => x.Inspection == inspection);

            inspectionEntity.AuditInspector = _appState.CurrentInspector;
            inspectionEntity.AuditAnnotation = string.Empty;
            inspectionEntity.AuditResult = request.Result;

            await _businessObjectManager.UpdateAsync(entity);

            await _businessObjectInspectionAuditHandler.ReplaceAsync(
                entity.UniqueName,
                inspectionEntity.Inspection,
                inspectionEntity.AuditDate,
                inspectionEntity.AuditTime,
                new InspectionAuditRequest
                {
                    Annotation = inspectionEntity.AuditAnnotation,
                    AuditDate = inspectionEntity.AuditDate,
                    AuditTime = inspectionEntity.AuditTime,
                    BusinessObject = entity.UniqueName,
                    BusinessObjectDisplayName = entity.DisplayName,
                    Inspection = inspectionEntity.Inspection,
                    InspectionDisplayName = inspectionEntity.InspectionDisplayName,
                    Inspector = inspectionEntity.AuditInspector,
                    Result = inspectionEntity.AuditResult
                });
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
                        Inspections = entity.Inspections.ToEmbeddedResponses(),
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
                    Inspections = entity.Inspections.ToEmbeddedResponses(),
                    Inspector = entity.Inspector,
                    DisplayName = entity.DisplayName,
                    UniqueName = entity.UniqueName
                }));

        public async ValueTask RefreshInspectionAsync(string inspection, RefreshInspectionRequest request)
        {
            var bussinesObjects = _businessObjectManager.GetQueryable()
                .Where(x => x.Inspections.Any(x => x.Inspection == inspection));

            foreach(var bussinesObject in bussinesObjects)
            {
                foreach(var bussinesObjectInspection in bussinesObject.Inspections)
                {
                    if (bussinesObjectInspection.Inspection == inspection)
                    {
                        bussinesObjectInspection.InspectionDisplayName = request.DisplayName;
                        bussinesObjectInspection.InspectionText = request.Text;
                        bussinesObjectInspection.InspectionActivated = request.Activated;

                        await _businessObjectInspectionAuditHandler.ReplaceAsync(
                            bussinesObject.UniqueName,
                            bussinesObjectInspection.Inspection,
                            bussinesObjectInspection.AuditDate,
                            bussinesObjectInspection.AuditTime,
                            new InspectionAuditRequest
                            {
                                Annotation = bussinesObjectInspection.AuditAnnotation,
                                AuditDate = bussinesObjectInspection.AuditDate,
                                AuditTime = bussinesObjectInspection.AuditTime,
                                BusinessObject = bussinesObject.UniqueName,
                                BusinessObjectDisplayName = bussinesObject.DisplayName,
                                Inspection = bussinesObjectInspection.Inspection,
                                InspectionDisplayName = bussinesObjectInspection.InspectionDisplayName,
                                Inspector = bussinesObjectInspection.AuditInspector,
                                Result = bussinesObjectInspection.AuditResult
                            });
                    }
                }

                await _businessObjectManager.UpdateAsync(bussinesObject);
            }
        }
    }
}