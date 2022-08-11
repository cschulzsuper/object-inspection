using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;
using Super.Paula.JsonConversion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Super.Paula.Client.Auditing
{
    public class BusinessObjectInspectionHandler : IBusinessObjectInspectionHandler
    {
        private readonly HttpClient _httpClient;

        public BusinessObjectInspectionHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask<BusinessObjectInspectionResponse> CreateAsync(string businessObject, BusinessObjectInspectionRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspections", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionResponse>())!;
        }

        public async ValueTask<BusinessObjectInspectionAuditResponse> CreateAuditAsync(string businessObject, string inspection, BusinessObjectInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspections/{inspection}/create-audit", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionAuditResponse>())!;
        }

        public async ValueTask<BusinessObjectInspectionAuditOmissionResponse> CreateAuditOmissionAsync(string businessObject, string inspection, BusinessObjectInspectionAuditOmissionRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspections/{inspection}/create-audit-schedule-omission", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionAuditOmissionResponse>())!;
        }

        public async ValueTask DeleteAsync(string businessObject, string inspection, string etag)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"business-objects/{businessObject}/inspections/{inspection}");
            request.Headers.Add("If-Match", etag);

            var responseMessage = await _httpClient.SendAsync(request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<BusinessObjectInspectionResponse> GetAllForBusinessObject(string businessObject)
        {
            var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}/inspections");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectInspectionResponse>(
                responseStream, CustomJsonSerializerOptions.WebResponse);

            await foreach (var responseItem in response)
            {
                yield return responseItem!;
            }
        }

        public async ValueTask<BusinessObjectInspectionResponse> GetAsync(string businessObject, string inspection)
        {
            var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}/inspections/{inspection}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionResponse>())!;
        }

        public async ValueTask ReplaceAsync(string businessObject, string inspection, BusinessObjectInspectionRequest request)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync($"business-objects/{businessObject}/inspections/{inspection}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<BusinessObjectInspectionAuditAnnotationResponse> ReplaceAuditAnnotationAsync(string businessObject, string inspection, BusinessObjectInspectionAuditAnnotationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspections/{inspection}/replace-audit-annotation", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionAuditAnnotationResponse>())!;
        }

        public async ValueTask<BusinessObjectInspectionAuditResponse> ReplaceAuditAsync(string businessObject, string inspection, BusinessObjectInspectionAuditRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspections/{inspection}/replace-audit", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionAuditResponse>())!;
        }

        public async ValueTask<BusinessObjectInspectionAuditScheduleResponse> ReplaceAuditScheduleAsync(string businessObject, string inspection, BusinessObjectInspectionAuditScheduleRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"business-objects/{businessObject}/inspections/{inspection}/replace-audit-schedule", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectInspectionAuditScheduleResponse>())!;
        }


        public ValueTask RecalculateInspectionAuditAppointmentsAsync(string businessObject)
        {
            throw new NotImplementedException();
        }
    }
}
