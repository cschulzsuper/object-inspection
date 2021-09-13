using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Super.Paula.Administration.Requests;
using Super.Paula.Authentication;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;
using Super.Paula.Inventory.Requests;
using Super.Paula.Inventory.Responses;

namespace Super.Paula.Inventory
{
    internal class BusinessObjectHandler : IBusinessObjectHandler
    {
        private readonly PaulaAuthenticationStateManager _paulaAuthenticationStateManager;
        private readonly HttpClient _httpClient;

        public BusinessObjectHandler(
            HttpClient httpClient,
            PaulaAuthenticationStateManager paulaAuthenticationStateManager,
            AppSettings appSettings)
        {
            _paulaAuthenticationStateManager = paulaAuthenticationStateManager;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", _paulaAuthenticationStateManager.GetAuthenticationBearer());
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

        public async ValueTask CreateInspectionAuditAsync(string businessObject, CreateInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/create-inspection-audit", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask DeleteAsync(string businessObject)
        {
            var responseMessage = await _httpClient.DeleteAsync($"business-objects/{businessObject}");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<BusinessObjectResponse> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync("business-objects");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectResponse>(
                responseStream,
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultBufferSize = 128
                });

            await foreach (var reponseItem in response)
            {
                yield return reponseItem!;
            }
        }

        public async IAsyncEnumerable<BusinessObjectResponse> GetAllForInspector(string inspector)
        {
            var responseMessage = await _httpClient.GetAsync($"inspectors/{inspector}/business-objects");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectResponse>(
                responseStream,
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultBufferSize = 128
                });

            await foreach (var reponseItem in response)
            {
                yield return reponseItem!;
            }
        }

        public async ValueTask<BusinessObjectResponse> GetAsync(string businessObject)
        {
            var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectResponse>())!;
        }

        public ValueTask RefreshInspectionAsync(string inspection, RefreshInspectionRequest request)
            => throw new NotSupportedException("This operation is not supported on the client side");

        public async ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync($"business-objects/{businessObject}", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<BusinessObjectResponse> Search(string? businessObject, string? inspector)
        {
            var queryValues = new List<string?>
            {
                businessObject != null ? $"business-object={businessObject}" : null,
                inspector != null ? $"inspector={inspector}" : null
            };

            var query = $"?{string.Join('&', queryValues.Where(x => x != null))}";

            var responseMessage = await _httpClient.GetAsync($"business-objects/search{query}");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectResponse>(
                responseStream,
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultBufferSize = 128
                });

            await foreach (var reponseItem in response)
            {
                yield return reponseItem!;
            }
        }
    }
}
