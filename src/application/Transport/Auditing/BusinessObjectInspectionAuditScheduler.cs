using Cronos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Auditing
{
    internal class BusinessObjectInspectionAuditScheduler : IBusinessObjectInspectionAuditScheduler
    {
        public void Schedule(BusinessObjectInspection inspection, (int Date, int Time) limit)
        {
            var auditSchedule = inspection.AuditSchedule;

            var appointmentsFromExpressions = auditSchedule.Expressions
                .SelectMany(expression => CalculateAppointmentsFromExpression(inspection, limit, expression))
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
            BusinessObjectInspection inspection, (int Date, int Time) limit,
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
                    GetIntervalEnd(limit),
                    fromInclusive: false,
                    toInclusive: true);

            return occurrences.Select(occurrence =>
            {
                var occurrenceNumbers = occurrence.ToNumbers();

                return new BusinessObjectInspectionAuditScheduleTimestamp
                {
                    PlannedAuditDate = occurrenceNumbers.day,
                    PlannedAuditTime = occurrenceNumbers.milliseconds
                };
            });


        }

        private static DateTime GetIntervalBegin(BusinessObjectInspection inspection)
        {
            var plannedAuditOriginNumbers = inspection.Audit.AuditDate != default
                ? (inspection.Audit.AuditDate, inspection.Audit.AuditTime)
                : (inspection.AssignmentDate, inspection.AssignmentTime);

            return plannedAuditOriginNumbers.ToDateTime()
                .AddMilliseconds(inspection.AuditSchedule.Threshold);
        }

        private static DateTime GetIntervalEnd((int Date, int Time) limit)
            => limit.ToDateTime();
    }
}
