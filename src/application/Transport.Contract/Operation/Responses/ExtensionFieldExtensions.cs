using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Operation.Responses;

public static class ExtensionFieldExtensions
{
    public static ExtensionFieldResponse ToResponse(this ExtensionField extensionField)
    {
        var response = new ExtensionFieldResponse
        {
            UniqueName = extensionField.UniqueName,
            DisplayName = extensionField.DisplayName,
            DataType = extensionField.DataType,
            DataName = extensionField.DataName,
        };

        return response;
    }

    public static ISet<ExtensionFieldResponse> ToResponse(this IEnumerable<ExtensionField> extensionFields)
        => extensionFields
            .Select(ToResponse)
            .ToHashSet();
}