using System;
using System.Collections.Generic;
using System.Linq;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory
{
    public static class BusinessObjectInspectionAuditScheduleValidator
    {
        public static (bool, Func<(string, FormattableString)>) CronExpressionIsNotEmpty(string cronExpression)
            => (!string.IsNullOrWhiteSpace(cronExpression),
                () => (nameof(cronExpression), $"Cron expression of audit schedule can not be empty"));

        public static (bool, Func<(string, FormattableString)>) CronExpressionIsCronExpression(string cronExpression)
            => (string.IsNullOrEmpty(cronExpression) || CronExpressionValidator.IsValid(cronExpression),
                () => (nameof(cronExpression), $"Cron expression '{cronExpression}' of audit schedule is not a valid cron expression"));
    }
}
