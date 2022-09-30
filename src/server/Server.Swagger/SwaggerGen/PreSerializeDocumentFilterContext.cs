using Microsoft.AspNetCore.Http;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerGen;

public class PreSerializeDocumentFilterContext
{
    public required HttpContext HttpContext { get; init; }
}
