using Super.Paula.Validation;
using System;

namespace Super.Paula.Application.Auditing
{
    public static class BusinessObjectInspectionAuditScheduleTimestampValidator
    {
        public static (bool, Func<(string, FormattableString)>) PlannedAuditDateIsPositive(int plannedAuditDate)
            => (DayNumberValidator.IsValid(plannedAuditDate),
                () => (nameof(plannedAuditDate), $"Planned audit date '{plannedAuditDate}' of timestamp must be positive"));

        public static (bool, Func<(string, FormattableString)>) PlannedAuditTimeIsInDayTimeRange(int plannedAuditTime)
            => (MillisecondsValidator.IsValid(plannedAuditTime),
                () => (nameof(plannedAuditTime), $"Planned audit time '{plannedAuditTime}' of timestamp must be positive and less than 86400000"));
    }
}
