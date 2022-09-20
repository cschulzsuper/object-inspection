using ChristianSchulz.ObjectInspection.Server.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace ChristianSchulz.ObjectInspection.Server.Swagger;

public static class SwaggerEndpointOptionsExtensions
{

    public static SwaggerEndpointOptions ConfigurePreSerializeFilters(this SwaggerEndpointOptions options)
    {
        options.AddPreSerializeSchemaFilter<BusinessObjectExtensionPreSerializeSchemaFilter>();
        options.AddPreSerializePathItemFilter<BadgeAuthorizationPreSerializePathItemFilter>();
        options.AddPreSerializeDocumentFilter<EmptyPathItemPreSerializeDocumentFilter>();

        return options;
    }
}