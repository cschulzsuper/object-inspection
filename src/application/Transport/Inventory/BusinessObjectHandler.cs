﻿using Super.Paula.Application.Communication;
using Super.Paula.Application.Communication.Requests;
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

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectHandler : IBusinessObjectHandler, IBusinessObjectEventHandler
    {
        private readonly IBusinessObjectManager _businessObjectManager;
        private readonly Lazy<IInspectionHandler> _inspectionHandler;
        private readonly Lazy<INotificationHandler> _notificationHandler;
        private readonly AppState _appState;
        private readonly IEventBus _eventBus;

        public BusinessObjectHandler(
            IBusinessObjectManager businessObjectManager,
            Lazy<IInspectionHandler> inspectionHandler,
            Lazy<INotificationHandler> notificationHandler,
            AppState appState,
            IEventBus eventBus)
        {
            _businessObjectManager = businessObjectManager;
            _inspectionHandler = inspectionHandler;
            _notificationHandler = notificationHandler;
            _appState = appState;
            _eventBus = eventBus;
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

            var oldEntityInspector = entity.Inspector;

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
                Text = inspection.Text
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

            await PublishBusinessObjectInspectionAsync(entity, inspection);
        }

        public async ValueTask ChangeInspectionAuditAsync(string businessObject, string inspection, ChangeInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var businessObjectInspection = entity.Inspections
                .Single(x => x.UniqueName == inspection);

            businessObjectInspection.AuditInspector = _appState.CurrentInspector;
            businessObjectInspection.AuditResult = request.Result;

            await _businessObjectManager.UpdateAsync(entity);

            await PublishBusinessObjectInspectionAsync(entity, businessObjectInspection);
        }

        public async ValueTask AnnotateInspectionAuditAsync(string businessObject, string inspection, AnnotateInspectionAuditRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var businessObjectInspection = entity.Inspections
                .Single(x => x.UniqueName == inspection);

            businessObjectInspection.AuditInspector = _appState.CurrentInspector;
            businessObjectInspection.AuditAnnotation = request.Annotation;

            await _businessObjectManager.UpdateAsync(entity);

            await PublishBusinessObjectInspectionAsync(entity, businessObjectInspection);
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
            var businessObjects = _businessObjectManager.GetQueryableWithInspection(inspection)
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
                        await PublishBusinessObjectInspectionAsync(businessObject, businessObjectInspection);
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

        private async ValueTask PublishBusinessObjectInspectionAsync(BusinessObject businessObject, BusinessObject.EmbeddedInspection inspection)
        {
            if (inspection.AuditDate == default ||
                inspection.AuditTime == default)
            {
                return;
            }

            var @event = new BusinessObjectInspectionEvent
            {
                BusinessObjectDisplayName = businessObject.DisplayName,
                Inspection = inspection.UniqueName,
                InspectionDisplayName = inspection.DisplayName,
                AuditInspector = inspection.AuditInspector,
                AuditDate = inspection.AuditDate,
                AuditTime = inspection.AuditTime,
                AuditResult = inspection.AuditResult,
                AuditAnnotation = inspection.AuditAnnotation
            };

            await _eventBus.PublishAsync(EventCategories.BusinessObjectInspectionAudit, businessObject.UniqueName, @event);
        }
    }
}