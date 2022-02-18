using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Super.Paula.Application.Administration.Exceptions;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Runtime;
using Super.Paula.Authorization;
using Super.Paula.Environment;

namespace Super.Paula.Application.Administration
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly IIdentityManager _identityManager;
        private readonly IConnectionManager _connectionManager;
        private readonly IPasswordHasher<Identity> _passwordHasher;
        private readonly ClaimsPrincipal _principal;

        public AuthenticationHandler(
            IIdentityManager identityManager,
            IConnectionManager connectionManager,
            IPasswordHasher<Identity> passwordHasher,
            ClaimsPrincipal principal)
        {
            _identityManager = identityManager;
            _connectionManager = connectionManager;
            _passwordHasher = passwordHasher;
            _principal = principal;
        }

        public async ValueTask ChangeSecretAsync(ChangeSecretRequest request)
        {
            var identity = _identityManager.GetQueryable()
                .Single(x => x.UniqueName == _principal.GetIdentity());

            var oldSecretVerification = _passwordHasher.VerifyHashedPassword(identity, identity.Secret, request.OldSecret);
            if(oldSecretVerification == PasswordVerificationResult.Failed)
            {
                throw new TransportException($"The old secret does not match");
            }

            identity.Secret = _passwordHasher.HashPassword(identity, request.NewSecret);

            await _identityManager.UpdateAsync(identity);
        }

        public async ValueTask RegisterAsync(RegisterRequest request)
        {
            var identity = new Identity
            {
                MailAddress = request.MailAddress,
                UniqueName = request.UniqueName
            };

            identity.Secret = _passwordHasher.HashPassword(identity, request.Secret);

            await _identityManager.InsertAsync(identity);
        }

        public async ValueTask<string> SignInAsync(SignInRequest request)
        {
            var identity = _identityManager.GetQueryable()
                .Single(x => x.UniqueName == request.Identity);

            var secretVerification = _passwordHasher.VerifyHashedPassword(identity, identity.Secret, request.Secret);

            switch (secretVerification)
            {
                case PasswordVerificationResult.Success:
                    break;

                case PasswordVerificationResult.SuccessRehashNeeded:
                    identity.Secret = _passwordHasher.HashPassword(identity, request.Secret);
                    await _identityManager.UpdateAsync(identity);
                    break;

                case PasswordVerificationResult.Failed:
                    throw new TransportException($"The secret does not match");
            }

            var connectionProof = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));

            _connectionManager.Trace(
                identity.UniqueName,
                connectionProof);

            var token = new Token
            {
                Identity = identity.UniqueName,
                Proof = connectionProof
            };

            return token.ToBase64String();
        }

        public async ValueTask SignOutAsync()
        {
            try
            {
                var inspector = _identityManager
                    .GetQueryable()
                    .Single(x => x.UniqueName == _principal.GetIdentity());

                _connectionManager.Forget(inspector.UniqueName);

                await Task.CompletedTask;
            } 
            catch(Exception exception)
            {
                throw new SignOutException($"Could not sign out", exception);
            }
        }
    }
}