using Microsoft.AspNetCore.Http;

namespace Super.Paula.Server.SwaggerGen
{
    public class PreSerializePathItemFilterContext
    {
        public required HttpContext HttpContext { get; init; }
    }
}
