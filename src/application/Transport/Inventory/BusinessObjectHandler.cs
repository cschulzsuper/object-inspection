using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Guidlines;
using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;
using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    internal class BusinessObjectHandler : IBusinessObjectHandler, IBusinessObjectEventHandler
    {
        private readonly IBusinessObjectManager _businessObjectManager;
        private readonly Lazy<IInspectionHandler> _inspectionHandler;
        private readonly Lazy<INotificationHandler> _notificationHandler;
        private readonly Lazy<IBusinessObjectInspectionAuditHandler> _businessObjectInspectionAuditHandler;
        private readonly AppState _appState;

        public BusinessObjectHandler(
            IBusinessObjectManager businessObjectManager,
            Lazy<IInspectionHandler> inspectionHandler,
            Lazy<INotificationHandler> notificationHandler,
            Lazy<IBusinessObjectInspectionAuditHandler> businessObjectInspectionAuditHandler,
            AppState appState)
        {
            _businessObjectManager = businessObjectManager;
            _inspectionHandler = inspectionHandler;
            _notificationHandler = notificationHandler;
            _businessObjectInspectionAuditHandler = businessObjectInspectionAuditHandler;
            _appState = appState;
        }

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

        public async ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request)
        {
            var entity = new BusinessObject
            {
                Inspector = request.Inspector,
                DisplayName = request.DisplayName,
                UniqueName = request.UniqueName
            };

            await _businessObjectManager.InsertAsync(entity);

            var (date, time) = DateTime.UtcNow.ToNumbers();

            await _notificationHandler.Value.CreateAsync(request.Inspector, new NotificationRequest
            {
                Date = date,
                Time = time,
                Target = $"business-objects/{request.UniqueName}",
                Text = $"You are now the inspector for {request.DisplayName}!"
            });

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

            var oldEntityDisplayName = entity.DisplayName;
            var oldEntityUniqueName = entity.UniqueName;
            var oldEntityInspector = entity.Inspector;

            entity.Inspector = request.Inspector;
            entity.DisplayName = request.DisplayName;
            entity.UniqueName = request.UniqueName;

            await _businessObjectManager.UpdateAsync(entity);

            if (oldEntityDisplayName != request.DisplayName ||
                oldEntityUniqueName != request.UniqueName)
            {
                foreach (var bussinesObjectInspection in entity.Inspections)
                {
                    if (bussinesObjectInspection.AuditDate != default &&
                        bussinesObjectInspection.AuditTime != default)
                    {
                        await _businessObjectInspectionAuditHandler.Value.ReplaceAsync(
                        entity.UniqueName,
                        bussinesObjectInspection.UniqueName,
                        bussinesObjectInspection.AuditDate,
                        bussinesObjectInspection.AuditTime,
                        new BusinessObjectInspectionAuditRequest
                        {
                            Annotation = bussinesObjectInspection.AuditAnnotation,
                            AuditDate = bussinesObjectInspection.AuditDate,
                            AuditTime = bussinesObjectInspection.AuditTime,
                            BusinessObjectDisplayName = entity.DisplayName,
                            Inspection = bussinesObjectInspection.UniqueName,
                            InspectionDisplayName = bussinesObjectInspection.DisplayName,
                            Inspector = bussinesObjectInspection.AuditInspector,
                            Result = bussinesObjectInspection.AuditResult
                        });
                    }
                }
            }

            if (oldEntityInspector != request.Inspector)
            {
                var (date, time) = DateTime.UtcNow.ToNumbers();

                await _notificationHandler.Value.CreateAsync(oldEntityInspector, new NotificationRequest
                {
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{request.UniqueName}",
                    Text = $"You are not longer the inspector for {request.DisplayName}!"
                });

                await _notificationHandler.Value.CreateAsync(request.Inspector, new NotificationRequest
                {
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{request.UniqueName}",
                    Text = $"You are now the inspector for {request.DisplayName}!"
                });
            }
        }

        public async ValueTask DeleteAsync(string businessObject)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            await _businessObjectManager.DeleteAsync(entity);
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

            await _businessObjectInspectionAuditHandler.Value.CreateAsync(
                entity.UniqueName,
                new BusinessObjectInspectionAuditRequest
                {
                    Annotation = inspection.AuditAnnotation,
                    AuditDate = inspection.AuditDate,
                    AuditTime = inspection.AuditTime,
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
                await _businessObjectInspectionAuditHandler.Value.ReplaceAsync(
                entity.UniqueName,
                bussinesObjectInspection.UniqueName,
                bussinesObjectInspection.AuditDate,
                bussinesObjectInspection.AuditTime,
                new BusinessObjectInspectionAuditRequest
                {
                    Annotation = bussinesObjectInspection.AuditAnnotation,
                    AuditDate = bussinesObjectInspection.AuditDate,
                    AuditTime = bussinesObjectInspection.AuditTime,
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
                await _businessObjectInspectionAuditHandler.Value.ReplaceAsync(
                entity.UniqueName,
                bussinesObjectInspection.UniqueName,
                bussinesObjectInspection.AuditDate,
                bussinesObjectInspection.AuditTime,
                new BusinessObjectInspectionAuditRequest
                {
                    Annotation = bussinesObjectInspection.AuditAnnotation,
                    AuditDate = bussinesObjectInspection.AuditDate,
                    AuditTime = bussinesObjectInspection.AuditTime,
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

        public async ValueTask ProcessAsync(string inspection, InspectionEvent @event)
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
                        bussinesObjectInspection.DisplayName = @event.DisplayName;
                        bussinesObjectInspection.Text = @event.Text;
                        bussinesObjectInspection.ActivatedGlobally = @event.Activated;

                        if (bussinesObjectInspection.AuditDate != default &&
                            bussinesObjectInspection.AuditTime != default)
                        {
                            await _businessObjectInspectionAuditHandler.Value.ReplaceAsync(
                                bussinesObject.UniqueName,
                                bussinesObjectInspection.UniqueName,
                                bussinesObjectInspection.AuditDate,
                                bussinesObjectInspection.AuditTime,
                                new BusinessObjectInspectionAuditRequest
                                {
                                    Annotation = bussinesObjectInspection.AuditAnnotation,
                                    AuditDate = bussinesObjectInspection.AuditDate,
                                    AuditTime = bussinesObjectInspection.AuditTime,
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