using Super.Paula.Application.Administration.Requests;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IAuthenticationHandler
    {
        ValueTask RegisterAsync(RegisterRequest request);
        ValueTask<string> SignInAsync(SignInRequest request);
        ValueTask SignOutAsync();
        ValueTask ChangeSecretAsync(ChangeSecretRequest request);
    }
}