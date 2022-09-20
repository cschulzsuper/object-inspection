using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Application.Authentication;
using ChristianSchulz.ObjectInspection.Application.Authentication.Exceptions;
using ChristianSchulz.ObjectInspection.Application.Authentication.Requests;
using ChristianSchulz.ObjectInspection.Application.Authentication.Responses;
using ChristianSchulz.ObjectInspection.Client.Security;

namespace ChristianSchulz.ObjectInspection.Client.Administration;

public class ExtendedAuthenticationRequestHandler : IAuthenticationRequestHandler
{
    private readonly ILogger<ExtendedAuthenticationRequestHandler> _logger;
    private readonly IAuthenticationRequestHandler _authenticationHandler;
    private readonly BadgeStorage _badgeStorage;

    public ExtendedAuthenticationRequestHandler(
        ILogger<ExtendedAuthenticationRequestHandler> logger,
        IAuthenticationRequestHandler authenticationHandler,
        BadgeStorage badgeStorage)
    {
        _logger = logger;
        _authenticationHandler = authenticationHandler;
        _badgeStorage = badgeStorage;
    }

    public async ValueTask VerifyAsync()
    {
        try
        {
            await _authenticationHandler.VerifyAsync();
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, $"Authentication invalid.");

            await _badgeStorage.SetAsync(null);
        }
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

        await _badgeStorage.SetAsync(response);

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
            _logger.LogWarning(exception, "Could not sign out gracefully.");
        }
        finally
        {
            await _badgeStorage.SetAsync(null);
        }
    }
}