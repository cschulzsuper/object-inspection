using System.Net;
using System.Text.Json;

namespace Super.Paula.ErrorHandling
{
    public static class ProblemDetailsResponseExtension
    {
        public static HttpResponseMessage RuleOutProblems(this HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == HttpStatusCode.BadRequest ||
                responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                var responseStream = responseMessage.Content.ReadAsStream();
                var response = JsonSerializer.Deserialize<ProblemDetails>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (response != null)
                {
                    throw new HttpRequestException(
                        response.Title,
                        null, 
                        responseMessage.StatusCode);
                }
            }

            return responseMessage;
        }
    }
}
