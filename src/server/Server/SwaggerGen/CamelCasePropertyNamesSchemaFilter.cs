using Microsoft.OpenApi.Models;
using ChristianSchulz.ObjectInspection.Shared;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerGen;

public class CamelCasePropertyNamesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var properties = schema.Properties.ToList();

        foreach (var property in properties)
        {
            schema.Properties.Remove(property);

            var camelCasePropertyName = CaseStyleConverter.FromPascalCaseToCamelCase(property.Key);

            schema.Properties.Add(camelCasePropertyName, property.Value);
        }
    }
}