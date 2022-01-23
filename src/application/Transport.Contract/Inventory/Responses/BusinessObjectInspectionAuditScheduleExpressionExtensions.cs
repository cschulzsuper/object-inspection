using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Inventory.Responses
{
    public static class BusinessObjectInspectionAuditScheduleExpressionExtensions
    {
        public static BusinessObjectInspectionAuditScheduleExpressionResponse ToResponse(this BusinessObjectInspectionAuditScheduleExpression expression)
        {
            var response = new BusinessObjectInspectionAuditScheduleExpressionResponse
            {
                CronExpression = expression.CronExpression
            };

            return response;
        }

        public static ISet<BusinessObjectInspectionAuditScheduleExpressionResponse> ToResponse(this IEnumerable<BusinessObjectInspectionAuditScheduleExpression> expressions)
            => expressions
                .Select(ToResponse)
                .ToHashSet();
    }
}
