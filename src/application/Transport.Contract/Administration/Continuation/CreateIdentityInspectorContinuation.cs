using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Continuation;

public record CreateIdentityInspectorContinuation(
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
    string Inspector,

    bool Activated)

    : ContinuationBase;