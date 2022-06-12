using Super.Paula.Application.Storage;
using Super.Paula.Application.Storage.Responses;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Client.Storage
{
    public class FileBlobHandler : IFileBlobHandler
    {
        private readonly HttpClient _httpClient;

        public FileBlobHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask DeleteAsync(string container, string uniqueName, string btag)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{container}/{uniqueName}");
            request.Headers.Add("If-Match", btag);

            var responseMessage = await _httpClient.SendAsync(request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<Stream> ReadAsync(string container, string uniqueName)
        {
            var responseMessage = await _httpClient.GetAsync($"{container}/{uniqueName}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsStreamAsync();
        }

        public ValueTask<FileBlobResponse> WriteAsync(Stream stream, string container)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<FileBlobResponse> WriteAsync(Stream stream, string container, string uniqueName, string? btag)
        {
            using var streamContent = new StreamContent(stream);
            
            if (btag != null)
            {
                streamContent.Headers.Add("If-Match", btag);
            }

            var responseMessage = await _httpClient.PutAsync($"{container}/{uniqueName}", streamContent);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<FileBlobResponse>())!;
        }
    }
}
