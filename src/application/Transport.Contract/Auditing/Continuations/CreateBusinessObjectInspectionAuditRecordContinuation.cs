using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Continuations;

public record CreateBusinessObjectInspectionAuditRecordContinuation(

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    string BusinessObject,

    [StringLength(140)]
    string BusinessObjectDisplayName,

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    string AuditInspector,

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    string Inspection,

    [StringLength(140)]
    string InspectionDisplayName,

    [StringLength(4000)]
    string AuditAnnotation,

    [AuditResult]
    string AuditResult,

    [DayNumber]
    int AuditDate,

    [Milliseconds]
    int AuditTime)

    : ContinuationBase;