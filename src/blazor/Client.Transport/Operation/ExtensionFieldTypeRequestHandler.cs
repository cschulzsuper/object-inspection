using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using ChristianSchulz.ObjectInspection.Shared.JsonConversion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace ChristianSchulz.ObjectInspection.Client.Operation;

public class ExtensionFieldTypeRequestHandler : IExtensionFieldTypeRequestHandler
{
    private readonly HttpClient _httpClient;

    public ExtensionFieldTypeRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async IAsyncEnumerable<string> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync($"extension-field-types");

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