using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Super.Paula.Application.Authentication.Exceptions;
using Super.Paula.Application.Authentication.Requests;
using Super.Paula.Application.Authentication.Responses;
using Super.Paula.BadgeSecurity;
using Super.Paula.Shared.Security;

namespace Super.Paula.Application.Authentication;

public class AuthenticationRequestHandler : IAuthenticationRequestHandler
{
    private readonly IIdentityManager _identityManager;
    private readonly IBadgeHandler _badgeHandler;
    private readonly IBadgeProofManager _badgeManager;
    private readonly IPasswordHasher<Identity> _passwordHasher;
    private readonly ClaimsPrincipal _user;

    public AuthenticationRequestHandler(
        IIdentityManager identityManager,
        IBadgeHandler badgeHandler,
        IBadgeProofManager badgeManager,
        IPasswordHasher<Identity> passwordHasher,
        ClaimsPrincipal principal)
    {
        _identityManager = identityManager;
        _badgeHandler = badgeHandler;
        _badgeManager = badgeManager;
        _passwordHasher = passwordHasher;
        _user = principal;
    }

    public async ValueTask ChangeSecretAsync(ChangeIdentitySecretRequest request)
    {
        var identity = await _identityManager.GetAsync(_user.Claims.GetIdentity());

        var oldSecretVerification = _passwordHasher.VerifyHashedPassword(identity, identity.Secret, request.OldSecret);
        if (oldSecretVerification == PasswordVerificationResult.Failed)
        {
            throw new TransportException($"The old secret does not match.");
        }

        identity.Secret = _passwordHasher.HashPassword(identity, request.NewSecret);

        await _identityManager.UpdateAsync(identity);
    }

    public async ValueTask RegisterAsync(RegisterIdentityRequest request)
    {
        var identity = new Identity
        {
            MailAddress = request.MailAddress,
            UniqueName = request.UniqueName
        };

        identity.Secret = _passwordHasher.HashPassword(identity, request.Secret);

        await _identityManager.InsertAsync(identity);
    }

    public async ValueTask<string> SignInAsync(string identity, SignInIdentityRequest request)
    {
        var entity = await _identityManager.GetAsync(identity);

        var secretVerification = _passwordHasher.VerifyHashedPassword(entity, entity.Secret, request.Secret);

        switch (secretVerification)
        {
            case PasswordVerificationResult.Success:
                break;

            case PasswordVerificationResult.SuccessRehashNeeded:
                entity.Secret = _passwordHasher.HashPassword(entity, request.Secret);
                await _identityManager.UpdateAsync(entity);
                break;

            case PasswordVerificationResult.Failed:
                throw new TransportException($"The secret does not match.");
        }

        var badge = _badgeHandler.Authorize(_user, "identity", entity);

        return badge;
    }

    public ValueTask SignOutAsync()
    {
        try
        {
            _badgeManager.Purge(_user);

            return ValueTask.CompletedTask;
        }
        catch (Exception exception)
        {
            throw new SignOutException($"Could not sign out.", exception);
        }
    }

    public ValueTask VerifyAsync()
        => ValueTask.CompletedTask;

    public async ValueTask<ResetIdentityResponse> ResetAsync(string identity, string etag)
    {
        var entity = await _identityManager.GetAsync(identity);

        entity.Secret = _passwordHasher.HashPassword(entity, "default");
        entity.ETag = etag;

        await _identityManager.UpdateAsync(entity);

        return new ResetIdentityResponse
        {
            ETag = entity.ETag
        };
    }
}