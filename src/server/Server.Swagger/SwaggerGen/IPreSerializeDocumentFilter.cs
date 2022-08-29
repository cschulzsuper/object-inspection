using Microsoft.OpenApi.Models;

namespace Super.Paula.Server.SwaggerGen
{
    public interface IPreSerializeDocumentFilter
    {
        void Apply(OpenApiDocument document, PreSerializeDocumentFilterContext documentContext);
    }
}
