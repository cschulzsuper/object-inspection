using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Super.Paula
{
    public static class TokenExtensions
    {
        public static string ToBase64String(this Token token)
            => Convert.ToBase64String(
                    JsonSerializer.SerializeToUtf8Bytes(token,
                        new JsonSerializerOptions(JsonSerializerDefaults.Web)
                        {
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }));

        public static Claim[] ToClaims(this Token token)
            => token
                .GetType()
                .GetProperties()
                .Select(x => (x.Name, x.GetValue(token)))
                .Where(x => x.Item2 != null)
                .Select(x => new Claim(x.Name, $"{x.Item2}"))
                .ToArray();

        public static Token? ToToken(this string token)
            => JsonSerializer.Deserialize<Token>(
                    Convert.FromBase64String(token));
    }
}
