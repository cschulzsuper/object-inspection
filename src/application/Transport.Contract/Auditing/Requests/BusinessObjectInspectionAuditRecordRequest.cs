﻿using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Requests;

public class BusinessObjectInspectionAuditRecordRequest
{
    public string ETag { get; set; } = string.Empty;

    [StringLength(4000)]
    public string Annotation { get; set; } = string.Empty;

    public int AuditDate { get; set; }

    public int AuditTime { get; set; }

    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string Inspection { get; set; } = string.Empty;

    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string Inspector { get; set; } = string.Empty;

    [Required]
    [AuditResult]
    public string Result { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string BusinessObjectDisplayName { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string InspectionDisplayName { get; set; } = string.Empty;
}