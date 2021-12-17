using System.Collections.Generic;
using System.Linq;
using Cronos;

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
                AuditDelayThreshold = inspection.AuditDelayThreshold,
                AuditThreshold = inspection.AuditThreshold,
                AuditSchedules = inspection.AuditSchedules.ToResponse()
            };

            response = ScheduleAudit(response, inspection);

            return response;
        }

        public static ISet<BusinessObjectInspectionResponse> ToResponse(this IEnumerable<BusinessObjectInspection> inspection)
            => inspection
                .Select(ToResponse)
                .ToHashSet();

        private static BusinessObjectInspectionResponse ScheduleAudit(BusinessObjectInspectionResponse response, BusinessObjectInspection inspection)
        {
            var auditSchedule = inspection.AuditSchedules
                .FirstOrDefault()?.CronExpression;

            if (!string.IsNullOrWhiteSpace(auditSchedule))
            {
                var schedule = CronExpression.Parse(auditSchedule);
                SchedulePlannedAudit(response, schedule, inspection);
            }

            return response;
        }

        private static void SchedulePlannedAudit(BusinessObjectInspectionResponse response, CronExpression schedule, BusinessObjectInspection inspection)
        {
            var plannedAuditOrigin = inspection.AuditDate != default
                ? (inspection.AuditDate, inspection.AuditTime)
                : (inspection.AssignmentDate, inspection.AssignmentTime);

            var plannedAuditThreshold = inspection.AuditThreshold;
            var plannedAuditOriginTimestamp = plannedAuditOrigin
                .ToDateTime()
                .AddMilliseconds(plannedAuditThreshold);

            var plannedAuditTimestamp = schedule.GetNextOccurrence(plannedAuditOriginTimestamp, inclusive: false);
            var (plannedAuditDate, plannedAuditTime) = plannedAuditTimestamp?.ToNumbers()
                                       ?? (default, default);

            response.PlannedAuditDate = plannedAuditDate;
            response.PlannedAuditTime = plannedAuditTime;
        }
    }
}
