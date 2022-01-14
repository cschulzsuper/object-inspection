using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IAccountHandler
    {
        ValueTask RegisterIdentityAsync(RegisterIdentityRequest request);
        ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request);
        ValueTask<string> SignInInspectorAsync(SignInInspectorRequest request);
        
        ValueTask VerifyAsync();
        ValueTask SignOutInspectorAsync();
        ValueTask ChangeSecretAsync(ChangeSecretRequest request);
        ValueTask<QueryAuthorizationsResponse> QueryAuthorizationsAsync();

        ValueTask<string> StartImpersonationAsync(StartImpersonationRequest request);
        ValueTask StopImpersonationAsync();
        ValueTask RepairChiefInspectorAsync(RepairChiefInspectorRequest request);
        ValueTask<AssessChiefInspectorDefectivenessResponse> AssessChiefInspectorDefectivenessAsync(AssessChiefInspectorDefectivenessRequest request);
    }
}