using System.Collections.Generic;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Application.Operation.Responses;

public static class DistinctionTypeFieldExtensions
{
    public static DistinctionTypeFieldResponse ToResponse(this DistinctionTypeField extensionField)
    {
        var response = new DistinctionTypeFieldResponse
        {
            ExtensionField = extensionField.ExtensionField
        };

        return response;
    }

    public static ISet<DistinctionTypeFieldResponse> ToResponse(this IEnumerable<DistinctionTypeField> extensionFields)
        => extensionFields
            .Select(ToResponse)
            .ToHashSet();
}