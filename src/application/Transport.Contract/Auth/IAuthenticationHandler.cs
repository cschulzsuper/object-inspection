using Super.Paula.Application.Auth.Requests;
using Super.Paula.Application.Auth.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auth
{
    public interface IAuthenticationHandler
    {
        ValueTask RegisterAsync(RegisterIdentityRequest request);
        ValueTask<string> SignInAsync(string identity, SignInIdentityRequest request);
        ValueTask SignOutAsync();
        ValueTask ChangeSecretAsync(ChangeIdentitySecretRequest request);
        ValueTask<ResetIdentityResponse> ResetAsync(string identity, string etag);
    }
}