using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client.Auditing
{
    public class BusinessObjectInspectionAuditHandler : IBusinessObjectInspectionAuditHandler
    {
        private readonly HttpClient _httpClient;

        public BusinessObjectInspectionAuditHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask<BusinessObjectInspectionAuditResponse> CreateAsync(string businessObject, BusinessObjectInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspection-audits", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionAuditResponse>())!;
        }

        public async ValueTask DeleteAsync(string businessObject, string inspection, int date, int time)
        {
            var responseMessage = await _httpClient.DeleteAsync($"business-objects/{businessObject}/inspection-audits/{inspection}/{date}/{time}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAll(string query, int skip, int take,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responseMessage = await _httpClient.GetAsync($"inspection-audits?q={query}&s={skip}&t={take}", cancellationToken);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
            var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectInspectionAuditResponse>(
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

        public async IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAllForBusinessObject(string businessObject, int skip, int take)
        {
            var responseMessage = await _httpClient.GetAsync($"business-object/{businessObject}/inspection-audits?s={skip}&t={take}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectInspectionAuditResponse>(
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

        public async ValueTask<SearchBusinessObjectInspectionAuditResponse> SearchAsync(string query)
        {
            var responseMessage = await _httpClient.GetAsync($"inspection-audits/search?q={query}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<SearchBusinessObjectInspectionAuditResponse>())!;
        }

        public async ValueTask<BusinessObjectInspectionAuditResponse> GetAsync(string businessObject, string inspection, int date, int time)
        {
            var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}/inspection-audits/{inspection}/{date}/{time}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionAuditResponse>())!;
        }

        public async ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspection-audits/{inspection}/{date}/{time}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
