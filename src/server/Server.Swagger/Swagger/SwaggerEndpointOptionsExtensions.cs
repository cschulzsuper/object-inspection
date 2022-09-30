using System;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using ChristianSchulz.ObjectInspection.Server.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace ChristianSchulz.ObjectInspection.Server.Swagger;

public static class SwaggerEndpointOptionsExtensions
{
    public static SwaggerEndpointOptions AddPreSerializeSchemaFilter<T>(this SwaggerEndpointOptions options)
        where T : class, IPreSerializeSchemaFilter
    {
        options.PreSerializeFilters.Add(PreSerializeSchemaFilterAction<T>);

        return options;
    }

    public static SwaggerEndpointOptions AddPreSerializePathItemFilter<T>(this SwaggerEndpointOptions options)
        where T : class, IPreSerializePathItemFilter
    {
        options.PreSerializeFilters.Add(PreSerializePathItemFilterAction<T>);

        return options;
    }

    public static SwaggerEndpointOptions AddPreSerializeDocumentFilter<T>(this SwaggerEndpointOptions options)
        where T : class, IPreSerializeDocumentFilter
    {
        options.PreSerializeFilters.Add(PreSerializeDocumentFilterAction<T>);

        return options;
    }

    private static void PreSerializeSchemaFilterAction<T>(OpenApiDocument doc, HttpRequest req)
        where T : class, IPreSerializeSchemaFilter
    {
        var filter = Activator.CreateInstance<T>();

        foreach (var schema in doc.Components.Schemas)
        {
            var context = new PreSerializeSchemaFilterContext
            {
                SchemaName = schema.Key,
                HttpContext = req.HttpContext
            };

            filter.Apply(schema.Value, context);
        }
    }

    private static void PreSerializePathItemFilterAction<T>(OpenApiDocument doc, HttpRequest req)
        where T : class, IPreSerializePathItemFilter
    {
        var filter = Activator.CreateInstance<T>();

        foreach (var path in doc.Paths)
        {
            var context = new PreSerializePathItemFilterContext
            {
                HttpContext = req.HttpContext
            };

            filter.Apply(path.Value, context);
        }
    }

    private static void PreSerializeDocumentFilterAction<T>(OpenApiDocument doc, HttpRequest req)
        where T : class, IPreSerializeDocumentFilter
    {
        var filter = Activator.CreateInstance<T>();

        var context = new PreSerializeDocumentFilterContext
        {
            HttpContext = req.HttpContext
        };

        filter.Apply(doc, context);
    }
}
