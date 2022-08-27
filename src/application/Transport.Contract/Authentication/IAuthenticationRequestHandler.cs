using System.Threading.Tasks;
using Super.Paula.Application.Authentication.Requests;
using Super.Paula.Application.Authentication.Responses;

namespace Super.Paula.Application.Authentication;

public interface IAuthenticationRequestHandler
{
    ValueTask RegisterAsync(RegisterIdentityRequest request);
    ValueTask<string> SignInAsync(string identity, SignInIdentityRequest request);
    ValueTask SignOutAsync();
    ValueTask VerifyAsync();
    ValueTask ChangeSecretAsync(ChangeIdentitySecretRequest request);
    ValueTask<ResetIdentityResponse> ResetAsync(string identity, string etag);
}