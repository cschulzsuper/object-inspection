using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Application.Authentication.Requests;
using ChristianSchulz.ObjectInspection.Application.Authentication.Responses;

namespace ChristianSchulz.ObjectInspection.Application.Authentication;

public interface IAuthenticationRequestHandler
{
    ValueTask RegisterAsync(RegisterIdentityRequest request);
    ValueTask<string> SignInAsync(string identity, SignInIdentityRequest request);
    ValueTask SignOutAsync();
    ValueTask VerifyAsync();
    ValueTask ChangeSecretAsync(ChangeIdentitySecretRequest request);
    ValueTask<ResetIdentityResponse> ResetAsync(string identity, string etag);
}