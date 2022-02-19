using Super.Paula.Validation;
using System;

namespace Super.Paula.Application.Inventory
{
    public static class BusinessObjectInspectionAuditScheduleExpressionValidator
    {
        public static (bool, Func<(string, FormattableString)>) CronExpressionIsNotEmpty(string cronExpression)
            => (!string.IsNullOrWhiteSpace(cronExpression),
                () => (nameof(cronExpression), $"Cron expression of expression can not be empty"));

        public static (bool, Func<(string, FormattableString)>) CronExpressionIsCronExpression(string cronExpression)
            => (string.IsNullOrEmpty(cronExpression) || CronExpressionValidator.IsValid(cronExpression),
                () => (nameof(cronExpression), $"Cron expression '{cronExpression}' of expression is not a valid cron expression"));
    }
}
