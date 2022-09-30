using Microsoft.OpenApi.Models;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerGen;

public interface IPreSerializePathItemFilter
{
    void Apply(OpenApiPathItem pathItem, PreSerializePathItemFilterContext pathItemContext);
}
