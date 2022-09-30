using Microsoft.OpenApi.Models;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerGen;

public interface IPreSerializeDocumentFilter
{
    void Apply(OpenApiDocument document, PreSerializeDocumentFilterContext documentContext);
}
