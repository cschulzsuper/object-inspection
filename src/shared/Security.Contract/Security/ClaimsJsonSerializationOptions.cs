using System.Text.Json;
using ChristianSchulz.ObjectInspection.Shared.JsonConversion;

namespace ChristianSchulz.ObjectInspection.Shared.Security;

public static class ClaimsJsonSerializerOptions
{
    public static readonly JsonSerializerOptions Options;

    static ClaimsJsonSerializerOptions()
    {
        Options = new JsonSerializerOptions(CustomJsonSerializerOptions.Default);
        Options.Converters.Add(new ClaimsJsonConverter());
    }
}
