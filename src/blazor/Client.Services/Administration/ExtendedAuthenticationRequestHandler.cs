using Microsoft.Extensions.Logging;
using Super.Paula.Application.Auth;
using Super.Paula.Application.Auth.Exceptions;
using Super.Paula.Application.Auth.Requests;
using Super.Paula.Application.Auth.Responses;
using Super.Paula.Client.Storage;
using Super.Paula.Shared.Authorization;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration;

public class ExtendedAuthenticationRequestHandler : IAuthenticationRequestHandler
{
    private readonly ILogger<ExtendedAuthenticationRequestHandler> _logger;
    private readonly IAuthenticationRequestHandler _authenticationHandler;
    private readonly ILocalStorage _localStorage;

    public ExtendedAuthenticationRequestHandler(
        ILogger<ExtendedAuthenticationRequestHandler> logger,
        IAuthenticationRequestHandler authenticationHandler,
        ILocalStorage localStorage)
    {
        _logger = logger;
        _authenticationHandler = authenticationHandler;
        _localStorage = localStorage;
    }

    public ValueTask ChangeSecretAsync(ChangeIdentitySecretRequest request)
        => _authenticationHandler.ChangeSecretAsync(request);

    public ValueTask RegisterAsync(RegisterIdentityRequest request)
        => _authenticationHandler.RegisterAsync(request);

    public ValueTask<ResetIdentityResponse> ResetAsync(string identity, string etag)
        => _authenticationHandler.ResetAsync(identity, etag);

    public async ValueTask<string> SignInAsync(string identity, SignInIdentityRequest request)
    {
        var response = await _authenticationHandler.SignInAsync(identity, request);

        var token = response.ToToken();
        await _localStorage.SetItemAsync("token", token);

        return response;
    }

    public async ValueTask SignOutAsync()
    {
        try
        {
            await _authenticationHandler.SignOutAsync();
        }
        catch (SignOutException exception)
        {
            _logger.LogWarning(exception, $"Could not sign out gracefully.");
        }
        finally
        {
            await _localStorage.RemoveItemAsync("token");
            await _localStorage.RemoveItemAsync("authorization-filter");
        }
    }
}