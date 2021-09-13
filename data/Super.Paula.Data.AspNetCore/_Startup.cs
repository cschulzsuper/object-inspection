using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Startup
    {
        public static IApplicationBuilder EnsurePaulaData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope
                    .ServiceProvider.GetRequiredService<PaulaContext>()
                    .Database.EnsureCreated();
            }

            return app;
        }
    }
}
