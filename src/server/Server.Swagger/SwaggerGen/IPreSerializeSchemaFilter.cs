using Microsoft.OpenApi.Models;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerGen;

public interface IPreSerializeSchemaFilter
{
    void Apply(OpenApiSchema schema, PreSerializeSchemaFilterContext schemaContext);
}
