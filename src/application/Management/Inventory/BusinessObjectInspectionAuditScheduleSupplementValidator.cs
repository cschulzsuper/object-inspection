﻿using System;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory
{
    public static class BusinessObjectInspectionAuditScheduleSupplementValidator
    {
        public static (bool, Func<(string, FormattableString)>) PlannedAuditDateIsPositive(int plannedAuditDate)
            => (DayNumberValidator.IsValid(plannedAuditDate),
                () => (nameof(plannedAuditDate), $"Planned audit date '{plannedAuditDate}' of adjustment must be positive"));

        public static (bool, Func<(string, FormattableString)>) PlannedAuditTimeIsInDayTimeRange(int plannedAuditTime)
            => (MillisecondsValidator.IsValid(plannedAuditTime),
                () => (nameof(plannedAuditTime), $"Planned audit time '{plannedAuditTime}' of adjustment must be positive and less than 86400000"));
    }
}
