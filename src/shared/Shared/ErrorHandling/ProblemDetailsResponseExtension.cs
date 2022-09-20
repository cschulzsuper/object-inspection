using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace ChristianSchulz.ObjectInspection.Shared.ErrorHandling;

public static class ProblemDetailsResponseExtension
{
    public static HttpResponseMessage RuleOutProblems(this HttpResponseMessage responseMessage)
    {
        if ((responseMessage.StatusCode == HttpStatusCode.BadRequest ||
             responseMessage.StatusCode == HttpStatusCode.NotFound) &&
            responseMessage.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var responseStream = responseMessage.Content.ReadAsStream();
            var response = JsonSerializer.Deserialize<ClientProblemDetails>(responseStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (response != null)
            {
                var formattable =
                    response.Extensions?.ContainsKey("titleFormat") == true &&
                    response.Extensions?.ContainsKey("titleArguments") == true;

                var errors = response.Extensions?.ContainsKey("errors") == true
                    ? (IDictionary<string, string[]>)response.Extensions["errors"]!
                    : null;

                return !formattable
                    ? throw new ProblemDetailsException(
                        response.Title ?? string.Empty,
                        errors,
                        responseMessage.StatusCode)

                    : throw new ProblemDetailsException(
                        (string)response.Extensions!["titleFormat"]!,
                        (object?[])response.Extensions["titleArguments"]!,
                        errors,
                        responseMessage.StatusCode);
            }
        }

        return responseMessage;
    }
}