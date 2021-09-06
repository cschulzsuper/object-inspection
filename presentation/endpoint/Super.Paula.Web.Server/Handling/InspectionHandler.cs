using Super.Paula.Data;
using Super.Paula.Aggregates.Inspections;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Server.Handling
{
    public class InspectionHandler : IInspectionHandler
    {
        private readonly IRepository<Inspection> _inspectionRepository;
        private readonly Lazy<IBusinessObjectHandler> _businessObjectHandler;
        private readonly IInspectionBusinessObjectHandler _inspectionBusinessObjectHandler;

        public InspectionHandler(
            IRepository<Inspection> inspectionRpository,
            Lazy<IBusinessObjectHandler> businessObjectHandler,
            IInspectionBusinessObjectHandler inspectionBusinessObjectHandler)
        {
            _inspectionRepository = inspectionRpository;
            _businessObjectHandler = businessObjectHandler;
            _inspectionBusinessObjectHandler = inspectionBusinessObjectHandler;
        }

        public async ValueTask ActivateAsync(string inspection)
        {
            var entity = await _inspectionRepository.GetByIdAsync(inspection);

            var refresh = entity.Activated != true;

            entity.Activated = true;

            await _inspectionRepository.UpdateAsync(entity);

            if (refresh)
            {
                await RefreshInspectionAsync(entity);
            }
        }

        public async ValueTask DeactivateAsync(string inspection)
        {
            var entity = await _inspectionRepository.GetByIdAsync(inspection);

            var refresh = entity.Activated != false;

            entity.Activated = false;

            await _inspectionRepository.UpdateAsync(entity);

            if (refresh)
            {
                await RefreshInspectionAsync(entity);
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

            await _inspectionRepository.InsertAsync(entity);

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
            var entity = await _inspectionRepository.GetByIdAsync(inspection);

            await _inspectionRepository.DeleteAsync(entity);
        }

        public IAsyncEnumerable<InspectionResponse> GetAll()
            => _inspectionRepository
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
            var entity = await _inspectionRepository.GetByIdAsync(inspection);

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
            var entity = await _inspectionRepository.GetByIdAsync(inspection);

            var refresh =
                entity.Activated != request.Activated ||
                entity.DisplayName != request.DisplayName ||
                entity.Text != request.Text;

            entity.Activated = request.Activated;
            entity.DisplayName = request.DisplayName;
            entity.Text = request.Text;
            entity.UniqueName = request.UniqueName;

            await _inspectionRepository.UpdateAsync(entity);

            if (refresh)
            {
                await RefreshInspectionAsync(entity);
            }
        }

        private async ValueTask RefreshInspectionAsync(Inspection inspection)
        {
            var request = new RefreshInspectionRequest
            {
                DisplayName = inspection.DisplayName,
                Text = inspection.Text,
                Activated = inspection.Activated
            };

            await _businessObjectHandler.Value.RefreshInspectionAsync(inspection.UniqueName, request);
            await _inspectionBusinessObjectHandler.RefreshInspectionAsync(inspection.UniqueName, request);
        }
    }
}