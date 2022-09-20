using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ChristianSchulz.ObjectInspection.BlobStorage;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddServerBlobStorage(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IBlobStorage, BlobStorage>();

        services.AddAzureClients(builder =>
        {
            builder
                .AddBlobServiceClient(connectionString)
                .WithName("ObjectInspectionBlobStorage")
                .ConfigureOptions(x => x.Retry.MaxRetries = 0);
        });

        return services;
    }
}