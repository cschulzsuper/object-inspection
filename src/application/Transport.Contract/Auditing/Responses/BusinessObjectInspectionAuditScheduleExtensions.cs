using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Auditing.Responses;

public static class BusinessObjectInspectionAuditScheduleExtensions
{
    public static BusinessObjectInspectionAuditScheduleResponse ToResponse(this BusinessObjectInspectionAuditSchedule auditSchedule)
    {
        var response = new BusinessObjectInspectionAuditScheduleResponse
        {
            Expressions = auditSchedule.Expressions.ToResponse(),
            Threshold = auditSchedule.Threshold,
            Omissions = auditSchedule.Omissions.ToResponse(),
            Additionals = auditSchedule.Additionals.ToResponse(),
            Appointments = auditSchedule.Appointments.ToResponse()
        };

        return response;
    }

    public static ISet<BusinessObjectInspectionAuditScheduleResponse> ToResponse(this IEnumerable<BusinessObjectInspectionAuditSchedule> auditSchedule)
        => auditSchedule
            .Select(ToResponse)
            .ToHashSet();
}