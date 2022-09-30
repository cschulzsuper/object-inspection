using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Security;

public class BadgeAuthenticationMessageHandler : DelegatingHandler
{
    private readonly BadgeStorage _badgeStorage;

    public BadgeAuthenticationMessageHandler(BadgeStorage badgeStorage)
    {
        _badgeStorage = badgeStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var verification = request.RequestUri?.AbsolutePath.EndsWith("identities/me/verify") == true;

        var badge = verification
            ? await _badgeStorage.GetOrDefaultLocalAsync()
            : await _badgeStorage.GetOrDefaultAsync();

        request.Headers.Authorization = !string.IsNullOrWhiteSpace(badge)
                ? new AuthenticationHeaderValue("Bearer", badge)
                : null;

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _badgeStorage.SetAsync(null);

            return new HttpResponseMessage();
        }

        return response;
    }
}