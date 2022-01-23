using System;
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
                AuditSchedule = inspection.AuditSchedule.ToResponse()
            };

            response = Schedule(response, inspection);

            return response;
        }

        public static ISet<BusinessObjectInspectionResponse> ToResponse(this IEnumerable<BusinessObjectInspection> inspection)
            => inspection
                .Select(ToResponse)
                .ToHashSet();



        private static BusinessObjectInspectionResponse Schedule(BusinessObjectInspectionResponse response, BusinessObjectInspection inspection)
        {
            var expression = inspection.AuditSchedule.Expressions
                .FirstOrDefault();

            var cronExpression = expression?.CronExpression;

            if (!string.IsNullOrWhiteSpace(cronExpression))
            {
                var schedule = CronExpression.Parse(cronExpression);
                ScheduleAppointment(response, schedule, inspection);
            }

            return response;
        }

        private static void ScheduleAppointment(BusinessObjectInspectionResponse response, CronExpression schedule, BusinessObjectInspection inspection)
        {
            var plannedAuditOriginNumbers = inspection.AuditDate != default
                ? (inspection.AuditDate, inspection.AuditTime)
                : (inspection.AssignmentDate, inspection.AssignmentTime);

            var plannedAuditOriginTimestamp = plannedAuditOriginNumbers.ToDateTime();

            while (true)
            {
                var plannedAuditTimestamp = GetNextOccurrence(schedule, inspection, plannedAuditOriginNumbers);

                var plannedAuditTimestampBegin = plannedAuditTimestamp
                    .AddMilliseconds(-inspection.AuditSchedule.Threshold);

                var plannedAuditTimestampEnd = plannedAuditTimestamp
                    .AddMilliseconds(inspection.AuditSchedule.Threshold);

                if (plannedAuditOriginTimestamp < plannedAuditTimestampBegin ||
                    plannedAuditOriginTimestamp > plannedAuditTimestampEnd)
                {
                    (int plannedAuditDate, int plannedAuditTime) = plannedAuditTimestamp.ToNumbers();

                    response.AuditSchedule.Appointments.Add(new BusinessObjectInspectionAuditScheduleTimestampResponse
                    {
                        PlannedAuditDate = plannedAuditDate,
                        PlannedAuditTime = plannedAuditTime
                    });
                    
                    break;
                }

                plannedAuditOriginNumbers = plannedAuditTimestamp.ToNumbers();
            }
        }

        private static DateTime GetNextOccurrence(CronExpression schedule, BusinessObjectInspection inspection, (int Date, int Time) plannedAuditOriginNumbers)
        {
            var plannedAuditOriginTimestamp = plannedAuditOriginNumbers.ToDateTime();

            var plannedAuditTimestamp = schedule.GetNextOccurrence(plannedAuditOriginTimestamp, inclusive: false)
                ?? plannedAuditOriginTimestamp;

            while (plannedAuditTimestamp != plannedAuditOriginTimestamp)
            {
                var plannedAuditNumbers = plannedAuditTimestamp.ToNumbers();

                var ignorePlannedAudit = inspection.AuditSchedule.Omissions
                    .Any(x =>
                        x.PlannedAuditDate == plannedAuditNumbers.day &&
                        x.PlannedAuditTime == plannedAuditNumbers.milliseconds);

                if (!ignorePlannedAudit)
                {
                    break;
                }

                plannedAuditTimestamp = schedule.GetNextOccurrence(plannedAuditTimestamp, inclusive: false) ?? plannedAuditOriginTimestamp;
            }

            var plannedAuditAdjusment = inspection.AuditSchedule.Additionals
                .OrderBy(x => x.PlannedAuditDate)
                .ThenBy(x => x.PlannedAuditTime)
                .FirstOrDefault(x =>
                    x.PlannedAuditDate > plannedAuditOriginNumbers.Date &&
                    x.PlannedAuditTime > plannedAuditOriginNumbers.Time);

            if (plannedAuditAdjusment != null)
            {
                var plannedAuditAdjusmentTimestamp = (plannedAuditAdjusment.PlannedAuditDate, plannedAuditAdjusment.PlannedAuditTime).ToDateTime();
                if (plannedAuditAdjusmentTimestamp < plannedAuditTimestamp)
                {
                    return plannedAuditAdjusmentTimestamp;
                }
            }

            return plannedAuditTimestamp;
        }

    }
}
