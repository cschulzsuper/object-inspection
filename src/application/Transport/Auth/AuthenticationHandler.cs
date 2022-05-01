using Microsoft.AspNetCore.Identity;
using Super.Paula.Application.Auth.Exceptions;
using Super.Paula.Application.Auth.Requests;
using Super.Paula.Application.Auth.Responses;
using Super.Paula.Application.Operation;
using Super.Paula.Authorization;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auth
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly IIdentityManager _identityManager;
        private readonly IConnectionManager _connectionManager;
        private readonly IPasswordHasher<Identity> _passwordHasher;
        private readonly ClaimsPrincipal _user;

        public AuthenticationHandler(
            IIdentityManager identityManager,
            IConnectionManager connectionManager,
            IPasswordHasher<Identity> passwordHasher,
            ClaimsPrincipal principal)
        {
            _identityManager = identityManager;
            _connectionManager = connectionManager;
            _passwordHasher = passwordHasher;
            _user = principal;
        }

        public async ValueTask ChangeSecretAsync(ChangeIdentitySecretRequest request)
        {
            var identity = await _identityManager.GetAsync(_user.GetIdentity());

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

            var connectionProof = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));

            _connectionManager.Trace(
                entity.UniqueName,
                connectionProof);

            var token = new Token
            {
                Identity = entity.UniqueName,
                Proof = connectionProof
            };

            return token.ToBase64String();
        }

        public async ValueTask SignOutAsync()
        {
            try
            {
                var inspector = await _identityManager.GetAsync(_user.GetIdentity());

                _connectionManager.Forget(inspector.UniqueName);

                await Task.CompletedTask;
            }
            catch (Exception exception)
            {
                throw new SignOutException($"Could not sign out.", exception);
            }
        }

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
}