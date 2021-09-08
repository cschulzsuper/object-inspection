using Super.Paula.Aggregates.Inventory;
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
        private readonly AppState _appState;

        public BusinessObjectHandler(
            IBusinessObjectManager businessObjectManager,
            Lazy<IInspectionHandler> inspectionHandler,
            IBusinessObjectInspectionAuditHandler businessObjectInspectionAuditHandler,
            AppState appState)
        {
            _businessObjectManager = businessObjectManager;
            _inspectionHandler = inspectionHandler;
            _businessObjectInspectionAuditHandler = businessObjectInspectionAuditHandler;
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
                    if (bussinesObjectInspection.AuditDate != default &&
                        bussinesObjectInspection.AuditTime != default)
                    {
                        await _businessObjectInspectionAuditHandler.ReplaceAsync(
                        entity.UniqueName,
                        bussinesObjectInspection.UniqueName,
                        bussinesObjectInspection.AuditDate,
                        bussinesObjectInspection.AuditTime,
                        new InspectionAuditRequest
                        {
                            Annotation = bussinesObjectInspection.AuditAnnotation,
                            AuditDate = bussinesObjectInspection.AuditDate,
                            AuditTime = bussinesObjectInspection.AuditTime,
                            BusinessObject = entity.UniqueName,
                            BusinessObjectDisplayName = entity.DisplayName,
                            Inspection = bussinesObjectInspection.UniqueName,
                            InspectionDisplayName = bussinesObjectInspection.DisplayName,
                            Inspector = bussinesObjectInspection.AuditInspector,
                            Result = bussinesObjectInspection.AuditResult
                        });
                    }
                }
            }

        }

        public async ValueTask AssignInspectionAsync(string businessObject, AssignInspectionRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);
            var inspection = await _inspectionHandler.Value.GetAsync(request.UniqueName);

            entity.Inspections.Add(new BusinessObject.EmbeddedInspection
            {
                Activated = true,
                ActivatedGlobally = inspection.Activated,

                UniqueName = inspection.UniqueName,
                DisplayName = inspection.DisplayName,
                Text = inspection.Text,
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

        public async ValueTask CreateInspectionAuditAsync(string businessObject, CreateInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var inspection = entity.Inspections
                .Single(inspection => inspection.UniqueName == request.Inspection);

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
                    Inspection = inspection.UniqueName,
                    InspectionDisplayName = inspection.DisplayName,
                    Inspector = inspection.AuditInspector,
                    Result = inspection.AuditResult
                });
        }

        public async ValueTask ChangeInspectionAuditAsync(string businessObject, string inspection, ChangeInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var bussinesObjectInspection = entity.Inspections
                .Single(x => x.UniqueName == inspection);

            bussinesObjectInspection.AuditInspector = _appState.CurrentInspector;
            bussinesObjectInspection.AuditResult = request.Result;

            await _businessObjectManager.UpdateAsync(entity);

            if (bussinesObjectInspection.AuditDate != default &&
                bussinesObjectInspection.AuditTime != default)
            {
                await _businessObjectInspectionAuditHandler.ReplaceAsync(
                entity.UniqueName,
                bussinesObjectInspection.UniqueName,
                bussinesObjectInspection.AuditDate,
                bussinesObjectInspection.AuditTime,
                new InspectionAuditRequest
                {
                    Annotation = bussinesObjectInspection.AuditAnnotation,
                    AuditDate = bussinesObjectInspection.AuditDate,
                    AuditTime = bussinesObjectInspection.AuditTime,
                    BusinessObject = entity.UniqueName,
                    BusinessObjectDisplayName = entity.DisplayName,
                    Inspection = bussinesObjectInspection.UniqueName,
                    InspectionDisplayName = bussinesObjectInspection.DisplayName,
                    Inspector = bussinesObjectInspection.AuditInspector,
                    Result = bussinesObjectInspection.AuditResult
                });
            }
        }

        public async ValueTask AnnotateInspectionAuditAsync(string businessObject, string inspection, AnnotateInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var bussinesObjectInspection = entity.Inspections
                .Single(x => x.UniqueName == inspection);

            bussinesObjectInspection.AuditInspector = _appState.CurrentInspector;
            bussinesObjectInspection.AuditAnnotation = request.Annotation;

            await _businessObjectManager.UpdateAsync(entity);

            if (bussinesObjectInspection.AuditDate != default &&
                bussinesObjectInspection.AuditTime != default)
            {
                await _businessObjectInspectionAuditHandler.ReplaceAsync(
                entity.UniqueName,
                bussinesObjectInspection.UniqueName,
                bussinesObjectInspection.AuditDate,
                bussinesObjectInspection.AuditTime,
                new InspectionAuditRequest
                {
                    Annotation = bussinesObjectInspection.AuditAnnotation,
                    AuditDate = bussinesObjectInspection.AuditDate,
                    AuditTime = bussinesObjectInspection.AuditTime,
                    BusinessObject = entity.UniqueName,
                    BusinessObjectDisplayName = entity.DisplayName,
                    Inspection = bussinesObjectInspection.UniqueName,
                    InspectionDisplayName = bussinesObjectInspection.DisplayName,
                    Inspector = bussinesObjectInspection.AuditInspector,
                    Result = bussinesObjectInspection.AuditResult
                });
            }
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
            // See https://github.com/dotnet/efcore/issues/17957
            // for the reason why AsEnumerable is necessary 

            var bussinesObjects = _businessObjectManager.GetQueryable()
                .AsEnumerable()
                .Where(x => x.Inspections.Any(x => x.UniqueName == inspection));

            foreach (var bussinesObject in bussinesObjects)
            {
                foreach(var bussinesObjectInspection in bussinesObject.Inspections)
                {
                    if (bussinesObjectInspection.UniqueName == inspection)
                    {
                        bussinesObjectInspection.DisplayName = request.DisplayName;
                        bussinesObjectInspection.Text = request.Text;
                        bussinesObjectInspection.ActivatedGlobally = request.Activated;

                        if (bussinesObjectInspection.AuditDate != default &&
                            bussinesObjectInspection.AuditTime != default)
                        {
                            await _businessObjectInspectionAuditHandler.ReplaceAsync(
                                bussinesObject.UniqueName,
                                bussinesObjectInspection.UniqueName,
                                bussinesObjectInspection.AuditDate,
                                bussinesObjectInspection.AuditTime,
                                new InspectionAuditRequest
                                {
                                    Annotation = bussinesObjectInspection.AuditAnnotation,
                                    AuditDate = bussinesObjectInspection.AuditDate,
                                    AuditTime = bussinesObjectInspection.AuditTime,
                                    BusinessObject = bussinesObject.UniqueName,
                                    BusinessObjectDisplayName = bussinesObject.DisplayName,
                                    Inspection = bussinesObjectInspection.UniqueName,
                                    InspectionDisplayName = bussinesObjectInspection.DisplayName,
                                    Inspector = bussinesObjectInspection.AuditInspector,
                                    Result = bussinesObjectInspection.AuditResult
                                });
                        }
                    }
                }

                await _businessObjectManager.UpdateAsync(bussinesObject);
            }
        }
    }
}