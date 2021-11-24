using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Migrator
{
    public interface IStep
    {
        Task ExecuteAsync();

        public static async Task ExecuteAsync<TStep>(IServiceProvider services)
            where TStep : IStep
        {
            using var scope = services.CreateScope();

            var scopedServices = scope.ServiceProvider;
            var scopedStep = scopedServices.GetRequiredService<TStep>();

            await scopedStep.ExecuteAsync();
        }
    }
}
