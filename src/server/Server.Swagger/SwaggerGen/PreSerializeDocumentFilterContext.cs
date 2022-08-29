using Microsoft.AspNetCore.Http;

namespace Super.Paula.Server.SwaggerGen
{
    public class PreSerializeDocumentFilterContext
    {
        public required HttpContext HttpContext { get; init; }
    }
}
