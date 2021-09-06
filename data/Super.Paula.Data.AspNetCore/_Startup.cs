using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Super.Paula.Data.AspNetCore
{
    public static class _Startup
    {
        public static IApplicationBuilder EnsurePaulaData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope
                    .ServiceProvider.GetRequiredService<Data.PaulaContext>()
                    .Database.EnsureCreated();
            }

            return app;
        }
    }
}
