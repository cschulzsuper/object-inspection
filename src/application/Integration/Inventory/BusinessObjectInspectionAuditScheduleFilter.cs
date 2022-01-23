using Cronos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Inventory
{
    internal class BusinessObjectInspectionAuditScheduleFilter : IBusinessObjectInspectionAuditScheduleFilter
    {
        public void Apply(BusinessObjectInspectionAuditScheduleFilterContext context)
            => Schedule(context);

        private static void Schedule(BusinessObjectInspectionAuditScheduleFilterContext context)
        {
            var auditSchedule = context.Inspection.AuditSchedule;

            var appointmentsFromExpressions = auditSchedule.Expressions
                .SelectMany(expression => CalculateAppointmentsFromExpression(context, expression))
                .ToList();

            var appointmentsWithOmissions = appointmentsFromExpressions
                .Concat(context.Inspection.AuditSchedule.Additionals)
                .ToList();

            var appointments = appointmentsWithOmissions
                .Where(appointment => !auditSchedule.Omissions
                    .Any(omission =>
                        omission.PlannedAuditDate == appointment.PlannedAuditDate &&
                        omission.PlannedAuditTime == appointment.PlannedAuditTime))
                .ToList();

            context.Inspection.AuditSchedule.Appointments.Clear();
            foreach (var appointment in appointments)
            {
                context.Inspection.AuditSchedule.Appointments.Add(appointment);
            }
        }

        private static IEnumerable<BusinessObjectInspectionAuditScheduleTimestamp> CalculateAppointmentsFromExpression(
            BusinessObjectInspectionAuditScheduleFilterContext context,
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
                    GetIntervalBegin(context),
                    GetIntervalEnd(context),
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

        private static DateTime GetIntervalBegin(BusinessObjectInspectionAuditScheduleFilterContext context)
        {
            var inspection = context.Inspection;

            var plannedAuditOriginNumbers = inspection.AuditDate != default
                ? (inspection.AuditDate, inspection.AuditTime)
                : (inspection.AssignmentDate, inspection.AssignmentTime);

            return plannedAuditOriginNumbers.ToDateTime()
                .AddMilliseconds(context.Inspection.AuditSchedule.Threshold);
        }

        private static DateTime GetIntervalEnd(BusinessObjectInspectionAuditScheduleFilterContext context)
            => context.Limit.ToDateTime();
    }
}
