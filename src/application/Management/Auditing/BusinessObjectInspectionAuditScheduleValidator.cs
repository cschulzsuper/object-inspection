using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public static class BusinessObjectInspectionAuditScheduleValidator
{

    public static (bool, Func<(string, FormattableString)>) ThresholdIsInDayTimeRange(int threshold)
    => (MillisecondsValidator.IsValid(threshold),
        () => (nameof(threshold), $"Threshold '{threshold}' of audit schedule must be positive and less than 86400000"));

    public static (bool, Func<(string, FormattableString)>) OmissionsUnique(ICollection<BusinessObjectInspectionAuditScheduleTimestamp> omissions)
        => (!omissions.GroupBy(x => (x.PlannedAuditDate, x.PlannedAuditTime)).Any(x => x.Count() > 1),
            () => (nameof(omissions), $"Omission duplicates are not allowed in audit schedule"));

    public static (bool, Func<(string, FormattableString)>) DelaysUnique(ICollection<BusinessObjectInspectionAuditScheduleTimestamp> delays)
        => (!delays.GroupBy(x => (x.PlannedAuditDate, x.PlannedAuditTime)).Any(x => x.Count() > 1),
            () => (nameof(delays), $"Delay duplicates are not allowed in audit schedule"));

    public static (bool, Func<(string, FormattableString)>) AdditionalsUnique(ICollection<BusinessObjectInspectionAuditScheduleTimestamp> additionals)
        => (!additionals.GroupBy(x => (x.PlannedAuditDate, x.PlannedAuditTime)).Any(x => x.Count() > 1),
            () => (nameof(additionals), $"Additional duplicates are not allowed in audit schedule"));

    public static (bool, Func<(string, FormattableString)>) AppointmentsUnique(ICollection<BusinessObjectInspectionAuditScheduleTimestamp> appointments)
        => (!appointments.GroupBy(x => (x.PlannedAuditDate, x.PlannedAuditTime)).Any(x => x.Count() > 1),
            () => (nameof(appointments), $"Appointment duplicates are not allowed in audit schedule"));
}