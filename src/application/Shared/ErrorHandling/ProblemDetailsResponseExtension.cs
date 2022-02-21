using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Super.Paula.ErrorHandling
{
    public static class ProblemDetailsResponseExtension
    {
        public static HttpResponseMessage RuleOutProblems(this HttpResponseMessage responseMessage)
        {
            if ((responseMessage.StatusCode == HttpStatusCode.BadRequest ||
                 responseMessage.StatusCode == HttpStatusCode.NotFound) &&
                responseMessage.Content.Headers.ContentType?.MediaType == "application/problem+json")
            {
                var responseStream = responseMessage.Content.ReadAsStream();
                var response = JsonSerializer.Deserialize<ProblemDetails>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (response != null)
                {
                    throw new ProblemDetailsException(
                        response.Title,
                        response.Errors,
                        responseMessage.StatusCode);
                }
            }

            return responseMessage;
        }
    }
}
