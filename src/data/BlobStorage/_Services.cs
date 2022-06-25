using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.BlobStorage
{
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
                    .WithName("PaulaBlobStorage");
            });

            return services;
        }
    }
}