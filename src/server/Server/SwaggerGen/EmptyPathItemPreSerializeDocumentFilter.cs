using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

namespace Super.Paula.Server.SwaggerGen
{
    public class EmptyPathItemPreSerializeDocumentFilter : IPreSerializeDocumentFilter
    {
        public void Apply(OpenApiDocument document, PreSerializeDocumentFilterContext documentContext)
        {
            var emptyPaths = document.Paths.Where(pathItems => !pathItems.Value.Operations.Any());

            foreach (var emptyPath in emptyPaths)
            {
                document.Paths.Remove(emptyPath.Key);
            }
        }
    }
}
