using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Administration.Continuation
{
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
        
        : ContinuationBase("create-identity-inspector");
}
