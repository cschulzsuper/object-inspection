using Super.Paula.Application.Inventory.Responses;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Paula.Application.Administration.Responses
{
    public class InspectorResponse
    {
        public string Identity { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
        public bool Activated { get; set; }

        public ISet<BusinessObjectInspectionResponse> BusinessObjects { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionResponse>();
    }
}