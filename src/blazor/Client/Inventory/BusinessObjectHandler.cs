using Super.Paula.Application.Inventory;
using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;
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

namespace Super.Paula.Client.Inventory
{
    public class BusinessObjectHandler : IBusinessObjectHandler
    {
        private readonly HttpClient _httpClient;

        public BusinessObjectHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask AssignInspectionAsync(string businessObject, AssignInspectionRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/assign-inspection", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask CancelInspectionAsync(string businessObject, CancelInspectionRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/cancel-inspection", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask ScheduleInspectionAuditAsync(string businessObject, string inspection, ScheduleInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/schedule-inspection-audit/{inspection}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask ChangeInspectionAuditAsync(string businessObject, string inspection, ChangeInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/change-inspection-audit/{inspection}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask AnnotateInspectionAuditAsync(string businessObject, string inspection, AnnotateInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/annotate-inspection-audit/{inspection}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("business-objects", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectResponse>())!;
        }

        public async ValueTask<CreateInspectionAuditResponse> CreateInspectionAuditAsync(string businessObject, CreateInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/create-inspection-audit", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<CreateInspectionAuditResponse>())!;
        }

        public async ValueTask DeleteAsync(string businessObject)
        {
            var responseMessage = await _httpClient.DeleteAsync($"business-objects/{businessObject}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<BusinessObjectResponse> GetAll(string query, int skip, int take,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responseMessage = await _httpClient.GetAsync($"business-objects?q={query}&s={skip}&t={take}", cancellationToken);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
            var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectResponse>(
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

        public async ValueTask<BusinessObjectResponse> GetAsync(string businessObject)
        {
            var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectResponse>())!;
        }

        public async ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync($"business-objects/{businessObject}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<SearchBusinessObjectResponse> SearchAsync(string query)
        {
            var responseMessage = await _httpClient.GetAsync($"business-objects/search?q={query}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<SearchBusinessObjectResponse>())!;
        }

        public async ValueTask<DropInspectionAuditResponse> DropInspectionAuditAsync(string businessObject, string inspection, DropInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/drop-inspection-audit/{inspection}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<DropInspectionAuditResponse>())!;
        }

        public ValueTask TimeInspectionAuditAsync(string businessObject)
            => throw new NotSupportedException("Clients can not time the business object inspection audit.");
    }
}
