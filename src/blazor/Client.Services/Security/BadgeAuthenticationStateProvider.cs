using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using ChristianSchulz.ObjectInspection.BadgeUsage;

namespace ChristianSchulz.ObjectInspection.Client.Security;

public class BadgeAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IBadgeEncoding _badgeEncoding;
    private readonly BadgeStorage _badgeStorage;
    private Task<AuthenticationState>? _authenticationState;

    public BadgeAuthenticationStateProvider(
        IBadgeEncoding badgeEncoding,
        BadgeStorage badgeStorage)
    {
        _badgeEncoding = badgeEncoding;
        _badgeStorage = badgeStorage;
        _badgeStorage.OnChange(OnChangedAsync);
    }

    private Task OnChangedAsync(string? badge)
    {
        _authenticationState = null;

        NotifyAuthenticationStateChanged(CreateAuthenticationStateAsync(badge));

        return Task.CompletedTask;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var badge = await _badgeStorage.GetOrDefaultAsync();

        _authenticationState ??= CreateAuthenticationStateAsync(badge);

        return await _authenticationState;
    }

    private async Task<AuthenticationState> CreateAuthenticationStateAsync(string? badge)
    {
        await Task.CompletedTask;

        if (badge == null)
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        Enumerable.Empty<Claim>())));
        }

        var claims = _badgeEncoding.Decode(badge);

        return new AuthenticationState(
            new ClaimsPrincipal(
                new ClaimsIdentity(claims, "badge")));
    }
}