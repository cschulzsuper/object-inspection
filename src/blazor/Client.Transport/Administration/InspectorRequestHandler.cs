using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Application.Administration.Requests;
using ChristianSchulz.ObjectInspection.Application.Administration.Responses;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using ChristianSchulz.ObjectInspection.Shared.JsonConversion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Administration;

public class InspectorRequestHandler : IInspectorRequestHandler
{
    private readonly HttpClient _httpClient;

    public InspectorRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<ActivateInspectorResponse> ActivateAsync(string inspector, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"inspectors/{inspector}/activate");
        request.Headers.TryAddWithoutValidation("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<ActivateInspectorResponse>())!;
    }

    public async ValueTask<InspectorResponse> CreateAsync(InspectorRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("inspectors", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<InspectorResponse>())!;
    }

    public async ValueTask<DeactivateInspectorResponse> DeactivateAsync(string inspector, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"inspectors/{inspector}/deactivate");
        request.Headers.TryAddWithoutValidation("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<DeactivateInspectorResponse>())!;
    }

    public async ValueTask DeleteAsync(string inspector, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"inspectors/{inspector}");
        request.Headers.TryAddWithoutValidation("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<InspectorResponse> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("inspectors");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<InspectorResponse>(
            responseStream, CustomJsonSerializerOptions.WebResponse);

        await foreach (var responseItem in response)
        {
            yield return responseItem!;
        }
    }

    public async IAsyncEnumerable<IdentityInspectorResponse> GetAllForIdentity(string identity)
    {
        var responseMessage = await _httpClient.GetAsync($"identities/{identity}/inspectors");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<IdentityInspectorResponse>(
            responseStream, CustomJsonSerializerOptions.WebResponse);

        await foreach (var responseItem in response)
        {
            yield return responseItem!;
        }
    }

    public async IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization)
    {
        var responseMessage = await _httpClient.GetAsync($"organizations/{organization}/inspectors");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<InspectorResponse>(
            responseStream, CustomJsonSerializerOptions.WebResponse);

        await foreach (var responseItem in response)
        {
            yield return responseItem!;
        }
    }

    public async ValueTask<InspectorResponse> GetAsync(string inspector)
    {
        var responseMessage = await _httpClient.GetAsync($"inspectors/{inspector}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<InspectorResponse>())!;
    }

    public async ValueTask<InspectorResponse> GetCurrentAsync()
    {
        var responseMessage = await _httpClient.GetAsync($"inspectors/me");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<InspectorResponse>())!;
    }

    public async ValueTask ReplaceAsync(string inspector, InspectorRequest request)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"inspectors/{inspector}", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }
}