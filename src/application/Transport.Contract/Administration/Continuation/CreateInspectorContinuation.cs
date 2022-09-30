using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Continuation;

public record CreateInspectorContinuation(
    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string Organization,

    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string UniqueName,

    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string Identity,

    bool OrganizationActivated,

    [StringLength(140)]
    string OrganizationDisplayName,

    bool Activated)

    : ContinuationBase;