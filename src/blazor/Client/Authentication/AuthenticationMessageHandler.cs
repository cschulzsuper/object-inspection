using Super.Paula.Authorization;
using Super.Paula.Client.Local;
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
            var token = (await _localStorage.GetItemAsync<Token>("token"))?
                .ToBase64String();

            request.Headers.Authorization = !string.IsNullOrWhiteSpace(token)
                    ? new AuthenticationHeaderValue("Bearer", token)
                    : null;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
