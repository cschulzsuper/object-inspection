using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Super.Paula.Client.Configuration
{
    public static class BlazorApiConfigurationExtensions
    {
        public static async Task<IConfigurationBuilder> AddBlazorApiConfigurationAsync(this IConfigurationBuilder builder, string url)
        {
            var provider = new BlazorApiConfigurationProvider(url);
            await provider.LoadAsync();

            var source = new BlazorApiConfigurationSource(provider);

            return builder.Add(source);
        }
    }
}
