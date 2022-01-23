using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Inventory.Responses
{
    public static class BusinessObjectInspectionAuditScheduleTimestampExtensions
    {
        public static BusinessObjectInspectionAuditScheduleTimestampResponse ToResponse(this BusinessObjectInspectionAuditScheduleTimestamp timestamp)
        {
            var response = new BusinessObjectInspectionAuditScheduleTimestampResponse
            {
                PlannedAuditDate = timestamp.PlannedAuditDate,
                PlannedAuditTime = timestamp.PlannedAuditTime
            };

            return response;
        }

        public static ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> ToResponse(this IEnumerable<BusinessObjectInspectionAuditScheduleTimestamp> timestamps)
            => timestamps
                .Select(ToResponse)
                .ToHashSet();
    }
}
