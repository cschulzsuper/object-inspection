using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Guidlines.Requests;
using Super.Paula.Application.Guidlines.Responses;
using Super.Paula.Application.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Guidlines
{
    internal class InspectionHandler : IInspectionHandler
    {
        private readonly IInspectionManager _inspectionManager;
        private readonly IEventBus _eventBus;

        public InspectionHandler(
            IInspectionManager inspectionManager,
            IEventBus eventBus)
        {
            _inspectionManager = inspectionManager;
            _eventBus = eventBus;
        }

        public async ValueTask ActivateAsync(string inspection)
        {
            var entity = await _inspectionManager.GetAsync(inspection);

            var required = entity.Activated != true;
            if (required)
            {
                entity.Activated = true;

                await _inspectionManager.UpdateAsync(entity);
                await NotifyAsync(entity);
            }
        }

        public async ValueTask DeactivateAsync(string inspection)
        {
            var entity = await _inspectionManager.GetAsync(inspection);

            var required = entity.Activated != false;
            if (required)
            {
                entity.Activated = false;

                await _inspectionManager.UpdateAsync(entity);
                await NotifyAsync(entity);
            }
        }

        public async ValueTask<InspectionResponse> CreateAsync(InspectionRequest request)
        {
            var entity = new Inspection
            {
                Activated = request.Activated,
                DisplayName = request.DisplayName,
                Text = request.Text,
                UniqueName = request.UniqueName
            };

            await _inspectionManager.InsertAsync(entity);

            return new InspectionResponse
            {
                Activated = entity.Activated,
                DisplayName = entity.DisplayName,
                Text = entity.Text,
                UniqueName = entity.UniqueName
            };
        }

        public async ValueTask DeleteAsync(string inspection)
        {
            var entity = await _inspectionManager.GetAsync(inspection);

            await _inspectionManager.DeleteAsync(entity);
        }

        public IAsyncEnumerable<InspectionResponse> GetAll()
            => _inspectionManager
                .GetAsyncEnumerable(query => query
                .Select(entity => new InspectionResponse
                {
                    Activated = entity.Activated,
                    DisplayName = entity.DisplayName,
                    Text = entity.Text,
                    UniqueName = entity.UniqueName
                }));

        public async ValueTask<InspectionResponse> GetAsync(string inspection)
        {
            var entity = await _inspectionManager.GetAsync(inspection);

            return new InspectionResponse
            {
                Activated = entity.Activated,
                DisplayName = entity.DisplayName,
                Text = entity.Text,
                UniqueName = entity.UniqueName
            };
        }

        public async ValueTask ReplaceAsync(string inspection, InspectionRequest request)
        {
            var entity = await _inspectionManager.GetAsync(inspection);

            var required =
                entity.Activated != request.Activated ||
                entity.DisplayName != request.DisplayName ||
                entity.Text != request.Text;

            if (required)
            {
                entity.Activated = request.Activated;
                entity.DisplayName = request.DisplayName;
                entity.Text = request.Text;
                entity.UniqueName = request.UniqueName;

                await _inspectionManager.UpdateAsync(entity);
                await NotifyAsync(entity);
            }
        }

        private async ValueTask NotifyAsync(Inspection inspection)
        {
            var @event = new InspectionEvent
            {
                DisplayName = inspection.DisplayName,
                Text = inspection.Text,
                Activated = inspection.Activated
            };

            await _eventBus.PublishAsync(EventCategories.BusinessObject, inspection.UniqueName, @event);
        }
    }
}