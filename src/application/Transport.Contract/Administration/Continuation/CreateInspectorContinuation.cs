using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration.Continuation
{
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

        bool Activated) : ContinuationBase(Guid.Empty);
}
