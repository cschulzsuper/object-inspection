using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Inventory.Responses
{
    public static class BusinessObjectInspectionExtensions
    {
        public static BusinessObjectInspectionResponse ToResponse(this BusinessObjectInspection inspection)
        {
            var response = new BusinessObjectInspectionResponse
            {
                UniqueName = inspection.UniqueName,
                Activated = inspection.Activated,
                AuditAnnotation = inspection.AuditAnnotation,
                AuditDate = inspection.AuditDate,
                AuditTime = inspection.AuditTime,
                AuditInspector = inspection.AuditInspector,
                AuditResult = inspection.AuditResult,
                ActivatedGlobally = inspection.ActivatedGlobally,
                DisplayName = inspection.DisplayName,
                Text = inspection.Text,
                AuditSchedule = inspection.AuditSchedule.ToResponse()
            };

            return response;
        }

        public static ISet<BusinessObjectInspectionResponse> ToResponse(this IEnumerable<BusinessObjectInspection> inspection)
            => inspection
                .Select(ToResponse)
                .ToHashSet();
    }
}
