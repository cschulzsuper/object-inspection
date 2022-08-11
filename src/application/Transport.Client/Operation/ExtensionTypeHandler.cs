using Super.Paula.Application.Operation;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;
using Super.Paula.JsonConversion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Super.Paula.Client.Operation
{
    public class ExtensionTypeHandler : IExtensionTypeHandler
    {
        private readonly HttpClient _httpClient;

        public ExtensionTypeHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async IAsyncEnumerable<string> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync($"extension-types");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<string>(
                responseStream, CustomJsonSerializerOptions.WebResponse);

            await foreach (var responseItem in response)
            {
                yield return responseItem!;
            }
        }


    }
}
