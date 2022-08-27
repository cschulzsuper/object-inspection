using Super.Paula.Application.Operation;
using Super.Paula.Application.Operation.Requests;
using Super.Paula.Application.Operation.Responses;
using Super.Paula.Shared.Environment;
using Super.Paula.Shared.ErrorHandling;
using Super.Paula.Shared.JsonConversion;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Super.Paula.Application.Operation.Exceptions;

namespace Super.Paula.Client.Operation;

public class ExtensionRequestHandler : IExtensionRequestHandler
{
    private readonly HttpClient _httpClient;

    public ExtensionRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<ExtensionResponse> CreateAsync(ExtensionRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("extensions", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<ExtensionResponse>())!;
    }

    public async ValueTask<ExtensionFieldCreateResponse> CreateFieldAsync(string aggregateType, ExtensionFieldRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync($"extensions/{aggregateType}/fields", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<ExtensionFieldCreateResponse>())!;
    }

    public async ValueTask DeleteAsync(string aggregateType, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"extensions/{aggregateType}");
        request.Headers.Add("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async ValueTask<ExtensionFieldDeleteResponse> DeleteFieldAsync(string aggregateType, string field, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"extensions/{aggregateType}/fields/{field}");
        request.Headers.Add("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<ExtensionFieldDeleteResponse>())!;
    }

    public async IAsyncEnumerable<ExtensionResponse> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync($"extensions");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<ExtensionResponse>(
            responseStream, CustomJsonSerializerOptions.WebResponse);

        await foreach (var responseItem in response)
        {
            yield return responseItem!;
        }
    }

    public async ValueTask<ExtensionResponse> GetAsync(string aggregateType)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"extensions/{aggregateType}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<ExtensionResponse>())!;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ExtensionNotFoundException($"Could not query extension {aggregateType}", exception);
        }

    }
}