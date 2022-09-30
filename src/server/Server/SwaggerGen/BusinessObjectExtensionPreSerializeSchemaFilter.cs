using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ChristianSchulz.ObjectInspection.Application.Inventory.Requests;
using ChristianSchulz.ObjectInspection.Application.Inventory.Responses;
using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerGen;

public class BusinessObjectExtensionPreSerializeSchemaFilter : IPreSerializeSchemaFilter
{
    public void Apply(OpenApiSchema schema, PreSerializeSchemaFilterContext schemaContext)
    {
        if (schemaContext.SchemaName != nameof(BusinessObjectRequest) &&
            schemaContext.SchemaName != nameof(BusinessObjectResponse))
        {
            return;
        }

        var user = schemaContext.HttpContext.User;
        if (!user.IsAuthenticatedInspector())
        {
            return;
        }

        var extensionManager = schemaContext.HttpContext.RequestServices.GetRequiredService<IExtensionManager>();
        var extension = extensionManager.GetAsync("business-object").Result;

        foreach (var extensionField in extension.Fields)
        {
            schema.Properties.Add(
                extensionField.DataName, 
                new OpenApiSchema
                {
                    Type = extensionField.DataType
                });
        }
    }
}
