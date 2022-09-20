using Microsoft.AspNetCore.Http;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerGen;

public class PreSerializePathItemFilterContext
{
    public required HttpContext HttpContext { get; init; }
}
