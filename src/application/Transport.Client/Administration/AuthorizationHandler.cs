using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        private readonly HttpClient _httpClient;

        public AuthorizationHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask<string> AuthorizeAsync(string organization, string inspector)
        {
            var responseMessage = await _httpClient.PostAsync($"organizations/{organization}/inspectors/{inspector}/authorize", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadAsStringAsync())!;
        }

        public async ValueTask<string> StartImpersonationAsync(string organization, string inspector)
        {
            var responseMessage = await _httpClient.PostAsync($"organizations/{organization}/inspectors/{inspector}/impersonate", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadAsStringAsync())!;
        }

        public async ValueTask<string> StopImpersonationAsync()
        {
            var responseMessage = await _httpClient.PostAsync("inspectors/current/unmask", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadAsStringAsync())!;
        }
    }
}