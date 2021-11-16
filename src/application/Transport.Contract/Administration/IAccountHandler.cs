using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IAccountHandler
    {
        ValueTask RegisterInspectorAsync(RegisterInspectorRequest request);
        ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request);
        ValueTask<SignInInspectorResponse> SignInInspectorAsync(SignInInspectorRequest request);
        
        ValueTask VerifyAsync();
        ValueTask SignOutInspectorAsync();
        ValueTask ChangeSecretAsync(ChangeSecretRequest request);
        ValueTask<QueryAuthorizationsResponse> QueryAuthorizationsAsync();

        ValueTask<StartImpersonationResponse> StartImpersonationAsync(StartImpersonationRequest request);
        ValueTask StopImpersonationAsync();
        ValueTask RepairChiefInspectorAsync(RepairChiefInspectorRequest request);
        ValueTask<AssessChiefInspectorDefectivenessResponse> AssessChiefInspectorDefectivenessAsync(AssessChiefInspectorDefectivenessRequest request);
    }
}