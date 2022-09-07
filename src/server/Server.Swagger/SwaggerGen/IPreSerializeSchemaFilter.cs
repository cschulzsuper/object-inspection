using Microsoft.OpenApi.Models;

namespace Super.Paula.Server.SwaggerGen
{
    public interface IPreSerializeSchemaFilter
    {
        void Apply(OpenApiSchema schema, PreSerializeSchemaFilterContext schemaContext);
    }
}
