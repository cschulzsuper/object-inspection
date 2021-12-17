using System;
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
    }
}
