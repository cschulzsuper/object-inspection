using Super.Paula.Server.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace Super.Paula.Server.Swagger;

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