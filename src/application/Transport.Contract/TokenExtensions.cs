using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Super.Paula.Application
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
        {
            var claims = token
                .GetType()
                .GetProperties()
                .Select(x => (x.Name, x.GetValue(token)))
                .Where(x => x.Item2 != null)
                .Select(x => new Claim(x.Name, $"{x.Item2}"))
                .ToList();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, $"{token.Organization}:{token.Inspector}"));

            return claims.ToArray();
        }

        public static Token? ToToken(this string token)
            => string.IsNullOrWhiteSpace(token) 
                    ? null
                    : JsonSerializer.Deserialize<Token>(
                            Convert.FromBase64String(token),
                            new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }
}
