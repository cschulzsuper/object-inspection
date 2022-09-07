using Microsoft.AspNetCore.Http;

namespace Super.Paula.Server.SwaggerGen
{
    public class PreSerializeSchemaFilterContext
    {
        public required HttpContext HttpContext { get; init; }

        public required string? SchemaName { get; init; }
    }
}
