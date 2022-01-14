
using Super.Paula.Client.Storage;
using Super.Paula.Environment;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client.Authentication
{
    public class AuthenticationMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorage _localStorage;

        public AuthenticationMessageHandler(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var appAuthentication = await _localStorage.GetItemAsync<AppAuthentication>("app-authentication");

            request.Headers.Authorization = !string.IsNullOrWhiteSpace(appAuthentication?.Token)
                    ? new AuthenticationHeaderValue("Bearer", appAuthentication.Token)
                    : null;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
