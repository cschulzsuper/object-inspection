using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Paula.Application.Inventory.Responses;

public class SearchBusinessObjectResponse
{
    public int TotalCount { get; set; }
    public ISet<BusinessObjectResponse> TopResults { get; set; } = ImmutableHashSet.Create<BusinessObjectResponse>();

}