
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client.Authentication
{
    public class AuthenticationMessageHandler : DelegatingHandler
    {
        private readonly AuthenticationStateManager _authenticationStateManager;

        public AuthenticationMessageHandler(AuthenticationStateManager authenticationStateManager)
        {
            _authenticationStateManager = authenticationStateManager;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var bearer = _authenticationStateManager.GetAuthenticationBearer();

            request.Headers.Authorization = !string.IsNullOrWhiteSpace(bearer)
                    ? new AuthenticationHeaderValue("Bearer", bearer)
                    : null;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
