using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Administration.Responses;

public static class InspectorBusinessObjectExtensions
{
    public static InspectorBusinessObjectResponse ToResponse(this InspectorBusinessObject businessObject)
    {
        var response = new InspectorBusinessObjectResponse
        {
            UniqueName = businessObject.UniqueName,
            DisplayName = businessObject.DisplayName,
            AuditScheduleDelayed = businessObject.AuditScheduleDelayed,
            AuditSchedulePending = businessObject.AuditSchedulePending,
            AuditSchedulePlannedAuditDate = businessObject.AuditSchedulePlannedAuditDate,
            AuditSchedulePlannedAuditTime = businessObject.AuditSchedulePlannedAuditTime
        };

        return response;
    }

    public static ISet<InspectorBusinessObjectResponse> ToResponse(this IEnumerable<InspectorBusinessObject> inspection)
        => inspection
            .Select(ToResponse)
            .ToHashSet();
}