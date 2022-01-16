using System;
using System.Collections.Generic;
using System.Linq;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory
{
    public static class BusinessObjectInspectionAuditScheduleValidator
    {
        public static (bool, Func<(string, FormattableString)>) CronExpressionIsNotNull(string cronExpression)
            => (cronExpression != null,
                () => (nameof(cronExpression), $"Cron expression of audit schedule can not be null"));

        public static (bool, Func<(string, FormattableString)>) CronExpressionIsCronExpression(string cronExpression)
            => (string.IsNullOrEmpty(cronExpression) || CronExpressionValidator.IsValid(cronExpression),
                () => (nameof(cronExpression), $"Cron expression '{cronExpression}' of audit schedule is not a valid cron expression"));

        internal static (bool, Func<(string, FormattableString)>) AdjustmentsUnique(IEnumerable<BusinessObjectInspectionAuditScheduleAdjustment> adjustments)
            => (adjustments.GroupBy(x => (x.PostponedAuditDate, x.PostponedAuditTime)).Any(x => x.Count() != 1),
                () => (nameof(adjustments), $"Adjustment duplicates are not allowed"));
    }
}
