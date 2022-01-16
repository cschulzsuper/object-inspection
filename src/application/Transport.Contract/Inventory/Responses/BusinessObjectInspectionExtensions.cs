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
                .FirstOrDefault();

            var cronExpression = auditSchedule?.CronExpression;

            if (!string.IsNullOrWhiteSpace(cronExpression))
            {
                var schedule = CronExpression.Parse(cronExpression);
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

            while(true) {

                var plannedAuditTimestamp = schedule.GetNextOccurrence(plannedAuditOriginTimestamp, inclusive: false);
                var (plannedAuditDate, plannedAuditTime) = plannedAuditTimestamp?.ToNumbers()
                                           ?? (default, default);

                response.PlannedAuditDate = plannedAuditDate;
                response.PlannedAuditTime = plannedAuditTime;

                var adjustment = inspection.AuditScheduleAdjustments?
                    .SingleOrDefault(x =>
                        x.PostponedAuditDate == response.PlannedAuditDate &&
                        x.PostponedAuditTime == response.PlannedAuditTime);

                if (adjustment?.PlannedAuditDate == default(int) &&
                    adjustment?.PlannedAuditTime == default(int) &&
                    plannedAuditTimestamp != null)
                {
                    plannedAuditOriginTimestamp = plannedAuditTimestamp.Value;
                    continue;
                }

                if (adjustment is not null &&
                    adjustment.PlannedAuditDate != default &&
                    adjustment.PlannedAuditTime != default)
                {
                    // TODO: This is not accurat we need to check if the next occurence is less than the postponed occurence.

                    adjustment.PostponedAuditDate = default;
                    adjustment.PostponedAuditTime = default;
                }

                break;
            }
        }
    }
}
