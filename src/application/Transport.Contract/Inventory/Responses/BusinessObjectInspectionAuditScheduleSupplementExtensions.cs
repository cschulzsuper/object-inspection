using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Inventory.Responses
{
    public static class BusinessObjectInspectionAuditScheduleSupplementExtensions
    {
        public static BusinessObjectInspectionAuditScheduleSupplementResponse ToResponse(this BusinessObjectInspectionAuditScheduleSupplement supplement)
        {
            var response = new BusinessObjectInspectionAuditScheduleSupplementResponse
            {
                PlannedAuditDate = supplement.PlannedAuditDate,
                PlannedAuditTime = supplement.PlannedAuditTime,
            };

            return response;
        }

        public static ISet<BusinessObjectInspectionAuditScheduleSupplementResponse> ToResponse(this IEnumerable<BusinessObjectInspectionAuditScheduleSupplement> supplements)
            => supplements
                .Select(ToResponse)
                .ToHashSet();
    }
}
