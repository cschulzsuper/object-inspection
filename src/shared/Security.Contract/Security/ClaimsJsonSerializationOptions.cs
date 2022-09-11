using System.Text.Json;
using Super.Paula.Shared.JsonConversion;

namespace Super.Paula.Shared.Security
{
    public static class ClaimsJsonSerializerOptions
    {
        public static readonly JsonSerializerOptions Options;

        static ClaimsJsonSerializerOptions()
        {
            Options = new JsonSerializerOptions(CustomJsonSerializerOptions.Default);
            Options.Converters.Add(new ClaimsJsonConverter());
        }
    }
}
