using Super.Paula.Data;
using Super.Paula.Management;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Super.Paula.Aggregates.InspectionBusinessObjects;
using Super.Paula.Web.Shared.Handling.Requests;

namespace Super.Paula.Web.Server.Handling
{
    public class InspectionBusinessObjectHandler : IInspectionBusinessObjectHandler
    {
        private readonly IRepository<InspectionBusinessObject> _inspectionBusinessObjectRepository;

        public InspectionBusinessObjectHandler(IRepository<InspectionBusinessObject> inspectionBusinessObjectRepository)
        {
            _inspectionBusinessObjectRepository = inspectionBusinessObjectRepository;
        }

        public async ValueTask<InspectionBusinessObjectResponse> CreateAsync(InspectionBusinessObjectRequest request)
        {
            var entity = new InspectionBusinessObject
            {
                BusinessObject = request.BusinessObject,
                Inspection = request.Inspection,
                InspectionActivated = request.InspectionActivated,
                InspectionDisplayName = request.InspectionDisplayName,
                InspectionText = request.InspectionText
            };

            await _inspectionBusinessObjectRepository.InsertAsync(entity);

            return new InspectionBusinessObjectResponse
            {
                BusinessObject = entity.BusinessObject,
                Inspection = entity.Inspection,
                InspectionActivated = entity.InspectionActivated,
                InspectionDisplayName = entity.InspectionDisplayName,
                InspectionText = entity.InspectionText
            };
        }

        public async ValueTask DeleteAsync(string inspection, string businessObject)
        {
            var entity = await _inspectionBusinessObjectRepository.GetByIdsAsync(inspection, businessObject);

            await _inspectionBusinessObjectRepository.DeleteAsync(entity);
        }

        public async ValueTask RefreshInspectionAsync(string inspection, RefreshInspectionRequest request)
        {
            var inspectionBusinessObjects = _inspectionBusinessObjectRepository.GetPartitionQueryable(inspection)
                .ToList();

            foreach(var inspectionBusinessObject in inspectionBusinessObjects)
            {
                inspectionBusinessObject.InspectionActivated = request.Activated;
                inspectionBusinessObject.InspectionDisplayName = request.DisplayName;
                inspectionBusinessObject.InspectionText = request.Text;

                await _inspectionBusinessObjectRepository.UpdateAsync(inspectionBusinessObject);

            }
        }
    }
}
