using Microsoft.OpenApi.Models;

namespace Super.Paula.Server.SwaggerGen
{
    public interface IPreSerializePathItemFilter
    {
        void Apply(OpenApiPathItem pathItem, PreSerializePathItemFilterContext pathItemContext);
    }
}
