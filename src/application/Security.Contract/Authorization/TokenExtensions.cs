using Super.Paula.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Super.Paula.Authorization
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
            var claims = Enumerable.Empty<Claim>()
                .Concat(GetSimplePropertyClaims(token))
                .Concat(GetArrayPropertyClaims(token))
                .ToList();

            if (!string.IsNullOrWhiteSpace(token.Organization) &&
                !string.IsNullOrWhiteSpace(token.Inspector))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, $"{token.Organization}:{token.Inspector}"));
            }

            return claims.ToArray();
        }

        private static IEnumerable<Claim> GetSimplePropertyClaims(Token token)
             => token
                .GetType()
                .GetProperties()
                .Where(x => !x.PropertyType.IsAssignableTo(typeof(IEnumerable<string>)))
                .Select(x => (x.Name, x.GetValue(token)))
                .Where(x => x.Item2 != null)
                .Select(x => new Claim(x.Name, $"{x.Item2}"));

        private static IEnumerable<Claim> GetArrayPropertyClaims(Token token)
             => token
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType.IsAssignableTo(typeof(IEnumerable<string>)))
                .Select(x => (
                    MakeSingular(x.Name),
                    (IEnumerable<string>?)x.GetValue(token)))
                .Where(x => x.Item2 != null)
                .SelectMany(x => x.Item2!.Select(y => new Claim(x.Item1, $"{y}")));

        private static string MakeSingular(string plural)
            => plural switch
            {
                nameof(Token.Authorizations) => "Authorization",
                _ => plural
            };
    }
}
