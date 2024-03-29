﻿namespace ChristianSchulz.ObjectInspection.Application.Auditing.Responses;

public class BusinessObjectInspectionAuditRecordResponse
{
    public string ETag { get; set; } = string.Empty;
    public string Annotation { get; set; } = string.Empty;
    public int AuditDate { get; set; }
    public string Inspection { get; set; } = string.Empty;
    public int AuditTime { get; set; }
    public string Inspector { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string BusinessObject { get; set; } = string.Empty;
    public string BusinessObjectDisplayName { get; set; } = string.Empty;
    public string InspectionDisplayName { get; set; } = string.Empty;
}