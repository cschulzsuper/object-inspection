using Super.Paula.Environment;
using Super.Paula.Web.Shared.Authentication;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Super.Paula.Web.Client.Handling
{
    public class BusinessObjectInspectionAuditHandler : IBusinessObjectInspectionAuditHandler
    {
        private readonly PaulaAuthenticationStateManager _paulaAuthenticationStateManager;
        private readonly HttpClient _httpClient;

        public BusinessObjectInspectionAuditHandler(
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

        public async ValueTask<InspectionAuditResponse> CreateAsync(string businessObject, InspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspection-audits", request);
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectionAuditResponse>())!;
        }

        public async ValueTask DeleteAsync(string businessObject, string inspection, int date, int time)
        {
            var responseMessage = await _httpClient.DeleteAsync($"business-objects/{businessObject}/inspection-audits/{inspection}/{date}/{time}");
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<InspectionAuditResponse> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync("inspection-audits");
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<InspectionAuditResponse>(
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

        public async IAsyncEnumerable<InspectionAuditResponse> GetAllForBusinessObject(string businessObject)
        {
            var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}/inspection-audits");
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<InspectionAuditResponse>(
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

        public async IAsyncEnumerable<InspectionAuditResponse> Search(string? businessObject, string? inspector, string? inspection)
        {
            var queryValues = new List<string?>
            {
                businessObject != null ? $"business-object={businessObject}" : null,
                inspector != null ? $"inspector={inspector}" : null,
                inspection != null ? $"inspection={inspection}" : null,
            };

            var query = $"?{string.Join('&', queryValues.Where(x => x != null))}";

            var responseMessage = await _httpClient.GetAsync($"inspection-audits/search{query}");
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<InspectionAuditResponse>(
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

        public async IAsyncEnumerable<InspectionAuditResponse> SearchForBusinessObject(string businessObject, string? inspector, string? inspection)
        {
            var queryValues = new List<string?>
            {
                inspector != null ? $"inspector={inspector}" : null,
                inspection != null ? $"inspection={inspection}" : null,
            };

            var query = $"?{string.Join('&', queryValues.Where(x => x != null))}";

            var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}/inspection-audits/search{query}");
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<InspectionAuditResponse>(
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

        public async ValueTask<InspectionAuditResponse> GetAsync(string businessObject, string inspection, int date, int time)
        {
            var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}/inspection-audits/{inspection}/{date}/{time}");
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectionAuditResponse>())!;
        }

        public async ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, InspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspection-audits/{inspection}/{date}/{time}", request);
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
