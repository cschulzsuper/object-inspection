using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Administration.Continuation;

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