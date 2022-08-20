using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Administration.Continuation;

public record ActivateIdentityInspectorContinuation(
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
    string Inspector)

    : ContinuationBase;