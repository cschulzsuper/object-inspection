using Super.Paula.Application.Administration.Requests;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IAccountHandler
    {
        ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request);
        ValueTask<string> SignInInspectorAsync(SignInInspectorRequest request);
        ValueTask<string> StartImpersonationAsync(StartImpersonationRequest request);
        ValueTask<string> StopImpersonationAsync();
    }
}