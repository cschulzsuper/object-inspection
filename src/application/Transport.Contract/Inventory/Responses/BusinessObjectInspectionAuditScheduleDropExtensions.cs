using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Inventory.Responses
{
    public static class BusinessObjectInspectionAuditScheduleDropExtensions
    {
        public static BusinessObjectInspectionAuditScheduleDropResponse ToResponse(this BusinessObjectInspectionAuditScheduleDrop drop)
        {
            var response = new BusinessObjectInspectionAuditScheduleDropResponse
            {
                PlannedAuditDate = drop.PlannedAuditDate,
                PlannedAuditTime = drop.PlannedAuditTime
            };

            return response;
        }

        public static ISet<BusinessObjectInspectionAuditScheduleDropResponse> ToResponse(this IEnumerable<BusinessObjectInspectionAuditScheduleDrop> drops)
            => drops
                .Select(ToResponse)
                .ToHashSet();
    }
}
