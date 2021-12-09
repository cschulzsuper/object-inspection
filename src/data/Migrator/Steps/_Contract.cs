using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Super.Paula.Data.Steps
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
