using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Super.Paula.Client.Storage;
using Super.Paula.Shared.Security;

namespace Super.Paula.Client.Security;

public class BadgeAuthenticationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorage _localStorage;

    public BadgeAuthenticationMessageHandler(ILocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var badge = (await _localStorage.GetItemAsync<Badge>("badge"))?
            .ToBase64String();

        request.Headers.Authorization = !string.IsNullOrWhiteSpace(badge)
                ? new AuthenticationHeaderValue("Bearer", badge)
                : null;

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _localStorage.RemoveItemAsync("badge");

            return new HttpResponseMessage();
        }

        return response;
    }
}