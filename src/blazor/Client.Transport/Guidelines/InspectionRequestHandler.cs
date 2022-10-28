using ChristianSchulz.ObjectInspection.Application.Guidelines;
using ChristianSchulz.ObjectInspection.Application.Guidelines.Requests;
using ChristianSchulz.ObjectInspection.Application.Guidelines.Responses;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Guidelines;

public class InspectionRequestHandler : IInspectionRequestHandler
{
    private readonly HttpClient _httpClient;

    public InspectionRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<ActivateInspectionResponse> ActivateAsync(string inspection, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"inspections/{inspection}/activate");
        request.Headers.TryAddWithoutValidation("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<ActivateInspectionResponse>())!;
    }

    public async ValueTask<InspectionResponse> CreateAsync(InspectionRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("inspections", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<InspectionResponse>())!;
    }

    public async ValueTask<DeactivateInspectionResponse> DeactivateAsync(string inspection, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"inspections/{inspection}/deactivate");
        request.Headers.TryAddWithoutValidation("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<DeactivateInspectionResponse>())!;
    }

    public async ValueTask DeleteAsync(string inspection, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"inspections/{inspection}");
        request.Headers.TryAddWithoutValidation("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<InspectionResponse> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("inspections");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<InspectionResponse>(
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

    public async ValueTask<InspectionResponse> GetAsync(string inspection)
    {
        var responseMessage = await _httpClient.GetAsync($"inspections/{inspection}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<InspectionResponse>())!;
    }

    public async ValueTask ReplaceAsync(string inspection, InspectionRequest request)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"inspections/{inspection}", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }
}