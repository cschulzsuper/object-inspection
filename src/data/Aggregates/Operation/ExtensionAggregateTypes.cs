namespace Super.Paula.Application.Operation;

public static class ExtensionAggregateTypes
{
    public readonly static string[] All = new string[]
    {
    Inspector,
    Inspection,
    BusinessObject,
    BusinessObjectInspection,
    BusinessObjectInspectionAuditRecord
    };

    public const string Inspector
        = "inspector";

    public const string Inspection
        = "inspection";

    public const string BusinessObject
        = "business-object";

    public const string BusinessObjectInspection
        = "business-object-inspection";

    public const string BusinessObjectInspectionAuditRecord
        = "business-object-inspection-audit-record";
}