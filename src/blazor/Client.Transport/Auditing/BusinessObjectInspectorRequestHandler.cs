using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using Super.Paula.Shared.Environment;
using Super.Paula.Shared.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Super.Paula.Client.Auditing;

public class BusinessObjectInspectorRequestHandler : IBusinessObjectInspectorRequestHandler
{
    private readonly HttpClient _httpClient;

    public BusinessObjectInspectorRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<BusinessObjectInspectorResponse> CreateAsync(string businessObject, BusinessObjectInspectorRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspectors", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectorResponse>())!;
    }

    public async ValueTask DeleteAsync(string businessObject, string inspector, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"business-objects/{businessObject}/inspectors/{inspector}");
        request.Headers.Add("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<BusinessObjectInspectorResponse> GetAllForBusinessObject(string businessObject)
    {
        var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}/inspectors");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectInspectorResponse>(
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

    public async ValueTask<BusinessObjectInspectorResponse> GetAsync(string businessObject, string inspector)
    {
        var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}/inspectors/{inspector}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectorResponse>())!;
    }

    public async ValueTask ReplaceAsync(string businessObject, string inspector, BusinessObjectInspectorRequest request)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"business-objects/{businessObject}/inspectors/{inspector}", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }
}