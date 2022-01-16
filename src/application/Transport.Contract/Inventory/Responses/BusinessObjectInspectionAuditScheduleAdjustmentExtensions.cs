using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Inventory.Responses
{
    public static class BusinessObjectInspectionAuditScheduleAdjustmentExtensions
    {
        public static BusinessObjectInspectionAuditScheduleAdjustmentResponse ToResponse(this BusinessObjectInspectionAuditScheduleAdjustment adjustment)
        {
            var response = new BusinessObjectInspectionAuditScheduleAdjustmentResponse
            {
                PlannedAuditDate = adjustment.PlannedAuditDate,
                PlannedAuditTime = adjustment.PlannedAuditTime,
                PostponedAuditDate = adjustment.PostponedAuditDate,
                PostponedAuditTime = adjustment.PostponedAuditTime,
            };

            return response;
        }

        public static ISet<BusinessObjectInspectionAuditScheduleAdjustmentResponse> ToResponse(this IEnumerable<BusinessObjectInspectionAuditScheduleAdjustment> adjustment)
            => adjustment
                .Select(ToResponse)
                .ToHashSet();
    }
}
