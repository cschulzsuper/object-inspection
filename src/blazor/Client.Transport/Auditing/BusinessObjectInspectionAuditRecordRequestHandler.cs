using ChristianSchulz.ObjectInspection.Application.Auditing;
using ChristianSchulz.ObjectInspection.Application.Auditing.Requests;
using ChristianSchulz.ObjectInspection.Application.Auditing.Responses;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Auditing;

public class BusinessObjectInspectionAuditRecordRequestHandler : IBusinessObjectInspectionAuditRecordRequestHandler
{
    private readonly HttpClient _httpClient;

    public BusinessObjectInspectionAuditRecordRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<BusinessObjectInspectionAuditRecordResponse> CreateAsync(string businessObject, BusinessObjectInspectionAuditRecordRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspection-audit-records", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionAuditRecordResponse>())!;
    }

    public async ValueTask DeleteAsync(string businessObject, string inspection, int date, int time, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"business-objects/{businessObject}/inspections/{inspection}/audit-records/{date}/{time}");
        request.Headers.Add("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<BusinessObjectInspectionAuditRecordResponse> GetAll(string query, int skip, int take,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var responseMessage = await _httpClient.GetAsync($"inspection-audit-records?q={query}&s={skip}&t={take}", cancellationToken);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
        var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectInspectionAuditRecordResponse>(
            responseStream,
            new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultBufferSize = 128
            },
            cancellationToken);

        await foreach (var responseItem in response
            .WithCancellation(cancellationToken))
        {
            yield return responseItem!;
        }
    }

    public async IAsyncEnumerable<BusinessObjectInspectionAuditRecordResponse> GetAllForBusinessObject(string businessObject, int skip, int take)
    {
        var responseMessage = await _httpClient.GetAsync($"business-object/{businessObject}/inspection-audit-records?s={skip}&t={take}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectInspectionAuditRecordResponse>(
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

    public async ValueTask<SearchBusinessObjectInspectionAuditRecordResponse> SearchAsync(string query)
    {
        var responseMessage = await _httpClient.GetAsync($"inspection-audit-records/search?q={query}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<SearchBusinessObjectInspectionAuditRecordResponse>())!;
    }

    public async ValueTask<BusinessObjectInspectionAuditRecordResponse> GetAsync(string businessObject, string inspection, int date, int time)
    {
        var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}/inspections/{inspection}/audit-records/{date}/{time}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionAuditRecordResponse>())!;
    }

    public async ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRecordRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspections/{inspection}/audit-records/{date}/{time}", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }
}