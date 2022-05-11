using Cronos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Auditing
{
    internal class BusinessObjectInspectionAuditScheduler : IBusinessObjectInspectionAuditScheduler
    {
        public void Schedule(BusinessObjectInspection inspection)
        {
            var auditScheduleDateTimeNumbersLimit = new DateTimeNumbers(DateTime.UtcNow.AddMonths(1));
            var auditScheduleCountLimit = 20;

            var auditSchedule = inspection.AuditSchedule;

            var appointmentsFromExpressions = auditSchedule.Expressions
                .SelectMany(expression => CalculateAppointmentsFromExpression(inspection, auditScheduleDateTimeNumbersLimit, auditScheduleCountLimit, expression))
                .ToList();

            var appointmentsWithOmissions = appointmentsFromExpressions
                .Concat(inspection.AuditSchedule.Additionals)
                .ToList();

            var appointments = appointmentsWithOmissions
                .Where(appointment => !auditSchedule.Omissions
                    .Any(omission =>
                        omission.PlannedAuditDate == appointment.PlannedAuditDate &&
                        omission.PlannedAuditTime == appointment.PlannedAuditTime))
                .ToList();

            inspection.AuditSchedule.Appointments.Clear();
            foreach (var appointment in appointments)
            {
                inspection.AuditSchedule.Appointments.Add(appointment);
            }
        }

        private static IEnumerable<BusinessObjectInspectionAuditScheduleTimestamp> CalculateAppointmentsFromExpression(
            BusinessObjectInspection inspection, DateTimeNumbers auditScheduleDateTimeNumbersLimit, int auditScheduleCountLimit,
            BusinessObjectInspectionAuditScheduleExpression expression)
        {
            var cronExpression = expression?.CronExpression;

            if (string.IsNullOrWhiteSpace(cronExpression))
            {
                return Enumerable.Empty<BusinessObjectInspectionAuditScheduleTimestamp>();
            }

            var occurrences = CronExpression
                .Parse(cronExpression)
                .GetOccurrences(
                    GetIntervalBegin(inspection),
                    GetIntervalEnd(auditScheduleDateTimeNumbersLimit),
                    fromInclusive: false,
                    toInclusive: true)
                .OrderBy(x => x)
                .Take(auditScheduleCountLimit);

            return occurrences.Select(occurrence =>
            {
                var occurrenceNumbers = new DateTimeNumbers(occurrence);

                return new BusinessObjectInspectionAuditScheduleTimestamp
                {
                    PlannedAuditDate = occurrenceNumbers.Date,
                    PlannedAuditTime = occurrenceNumbers.Time
                };
            });


        }

        private static DateTime GetIntervalBegin(BusinessObjectInspection inspection)
        {
            var plannedAuditOriginNumbers = inspection.Audit.AuditDate != default
                ? new DateTimeNumbers(inspection.Audit.AuditDate, inspection.Audit.AuditTime)
                : new DateTimeNumbers(inspection.AssignmentDate, inspection.AssignmentTime);

            return plannedAuditOriginNumbers
                .ToGlobalDateTime()
                .AddMilliseconds(inspection.AuditSchedule.Threshold);
        }

        private static DateTime GetIntervalEnd(DateTimeNumbers auditScheduleLimit)
            => auditScheduleLimit.ToGlobalDateTime();
    }
}
