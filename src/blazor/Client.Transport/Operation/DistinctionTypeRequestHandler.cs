using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.Application.Operation.Requests;
using ChristianSchulz.ObjectInspection.Application.Operation.Responses;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using ChristianSchulz.ObjectInspection.Shared.JsonConversion;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Application.Operation.Exceptions;

namespace ChristianSchulz.ObjectInspection.Client.Operation;

public class DistinctionTypeRequestHandler : IDistinctionTypeRequestHandler
{
    private readonly HttpClient _httpClient;

    public DistinctionTypeRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<DistinctionTypeResponse> CreateAsync(DistinctionTypeRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("distinction-types", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<DistinctionTypeResponse>())!;
    }

    public async ValueTask<DistinctionTypeFieldCreateResponse> CreateFieldAsync(string uniqueName, DistinctionTypeFieldRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync($"distinction-types/{uniqueName}/fields", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<DistinctionTypeFieldCreateResponse>())!;
    }

    public async ValueTask DeleteAsync(string uniqueName, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"distinction-types/{uniqueName}");
        request.Headers.Add("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async ValueTask<DistinctionTypeFieldDeleteResponse> DeleteFieldAsync(string uniqueName, string field, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"distinction-types/{uniqueName}/fields/{field}");
        request.Headers.Add("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<DistinctionTypeFieldDeleteResponse>())!;
    }

    public async IAsyncEnumerable<DistinctionTypeResponse> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync($"distinction-types");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<DistinctionTypeResponse>(
            responseStream, CustomJsonSerializerOptions.WebResponse);

        await foreach (var responseItem in response)
        {
            yield return responseItem!;
        }
    }

    public async ValueTask<DistinctionTypeResponse> GetAsync(string uniqueName)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"distinction-types/{uniqueName}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<DistinctionTypeResponse>())!;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            throw new DistinctionTypeNotFoundException($"Could not query extension {uniqueName}", exception);
        }

    }
}