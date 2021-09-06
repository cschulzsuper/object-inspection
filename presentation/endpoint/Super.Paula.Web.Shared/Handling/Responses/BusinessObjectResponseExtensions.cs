using Super.Paula.Aggregates.BusinessObjects;
using Super.Paula.Web.Shared.Handling.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Web.Shared.Handling.Responses
{
    public static class BusinessObjectResponseExtensions
    {
        public static BusinessObjectResponseInspection ToEmbeddedResponse(this BusinessObjectInspection inspection)
            => new BusinessObjectResponseInspection
            {
                Inspection = inspection.Inspection,
                Activated = inspection.Activated,
                AuditAnnotation = inspection.AuditAnnotation,
                AuditDate = inspection.AuditDate,
                AuditInspector = inspection.AuditInspector,
                AuditResult = inspection.AuditResult,
                AuditTime = inspection.AuditTime,
                InspectionActivated = inspection.InspectionActivated,
                InspectionDisplayName = inspection.InspectionDisplayName,
                InspectionText = inspection.InspectionText
            };

        public static ISet<BusinessObjectResponseInspection> ToEmbeddedResponses(this ISet<BusinessObjectInspection> inspection)
            => inspection
                .Select(ToEmbeddedResponse)
                .ToHashSet();
    }
}
