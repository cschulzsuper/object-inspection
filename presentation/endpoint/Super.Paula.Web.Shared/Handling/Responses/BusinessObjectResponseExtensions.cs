using Super.Paula.Web.Shared.Handling.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Super.Paula.Aggregates.Inventory;

namespace Super.Paula.Web.Shared.Handling.Responses
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
