using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Application.Authentication;
using ChristianSchulz.ObjectInspection.Application.Authentication.Requests;
using ChristianSchulz.ObjectInspection.Application.Authentication.Responses;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;

namespace ChristianSchulz.ObjectInspection.Client.Authentication;

public class IdentityRequestHandler : IIdentityRequestHandler
{
    private readonly HttpClient _httpClient;

    public IdentityRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<IdentityResponse> CreateAsync(IdentityRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("identities", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<IdentityResponse>())!;
    }

    public async ValueTask DeleteAsync(string identity, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"identities/{identity}");
        request.Headers.TryAddWithoutValidation("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<IdentityResponse> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("identities");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<IdentityResponse>(
            responseStream,
            new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultBufferSize = 128
            });

        await foreach (var responseItem in response)
        {
            yield return responseItem!;
        }
    }

    public async ValueTask<IdentityResponse> GetAsync(string identity)
    {
        var responseMessage = await _httpClient.GetAsync($"identities/{identity}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<IdentityResponse>())!;
    }

    public async ValueTask ReplaceAsync(string identity, IdentityRequest request)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"identities/{identity}", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }
}