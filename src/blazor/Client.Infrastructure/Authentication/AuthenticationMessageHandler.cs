
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
        private readonly AppAuthentication _appAuthentication;

        public AuthenticationMessageHandler(AppAuthentication appAuthentication)
        {
            _appAuthentication = appAuthentication;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = !string.IsNullOrWhiteSpace(_appAuthentication?.Bearer)
                    ? new AuthenticationHeaderValue("Bearer", _appAuthentication.Bearer)
                    : null;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
