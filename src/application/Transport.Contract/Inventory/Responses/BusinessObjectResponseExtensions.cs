using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Inventory.Responses
{
    public static class BusinessObjectResponseExtensions
    {
        public static BusinessObjectResponse.EmbeddedInspection ToEmbeddedResponse(this BusinessObject.EmbeddedInspection inspection)
            => new()
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
                Text = inspection.Text
            };

        public static ISet<BusinessObjectResponse.EmbeddedInspection> ToEmbeddedResponses(this ISet<BusinessObject.EmbeddedInspection> inspection)
            => inspection
                .Select(ToEmbeddedResponse)
                .ToHashSet();
    }
}
