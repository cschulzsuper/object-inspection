using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Super.Paula
{
    public static class BearerExtensions
    {
        public static string ToBase64String(this Bearer bearer) =>
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(bearer,
                        new JsonSerializerOptions(JsonSerializerDefaults.Web)
                        {
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        })));
    }
}
