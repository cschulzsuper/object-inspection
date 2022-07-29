using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Setup.Responses
{
    public static class ExtensionFieldExtensions
    {
        public static ExtensionFieldResponse ToResponse(this ExtensionField extensionField)
        {
            var response = new ExtensionFieldResponse
            {
                Name = extensionField.Name,
                Type = extensionField.Type
            };

            return response;
        }

        public static ISet<ExtensionFieldResponse> ToResponse(this IEnumerable<ExtensionField> extensionFields)
            => extensionFields
                .Select(ToResponse)
                .ToHashSet();
    }
}
