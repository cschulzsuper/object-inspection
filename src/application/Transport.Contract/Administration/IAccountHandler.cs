using Super.Paula.Application.Administration.Requests;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IAccountHandler
    {
        ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request);
        ValueTask RegisterChiefInspectorAsync(string organization, RegisterChiefInspectorRequest request);
        ValueTask<string> SignInInspectorAsync(string organization, string inspector);
        ValueTask<string> StartImpersonationAsync(string organization, string inspector);
        ValueTask<string> StopImpersonationAsync();
    }
}