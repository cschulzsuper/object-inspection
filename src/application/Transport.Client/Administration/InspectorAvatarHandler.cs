using Super.Paula.Application.Administration;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class InspectorAvatarHandler : IInspectorAvatarHandler
    {
        private readonly HttpClient _httpClient;

        public InspectorAvatarHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask DeleteAsync(string uniqueName)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"inspectors/{uniqueName}/avatar");

            var responseMessage = await _httpClient.SendAsync(request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<Stream> ReadAsync(string uniqueName)
        {
            var responseMessage = await _httpClient.GetAsync($"inspectors/{uniqueName}/avatar");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsStreamAsync();
        }

        public async ValueTask WriteAsync(Stream stream, string uniqueName)
        {
            using var streamContent = new StreamContent(stream);

            var responseMessage = await _httpClient.PutAsync($"inspectors/{uniqueName}/avatar", streamContent);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
