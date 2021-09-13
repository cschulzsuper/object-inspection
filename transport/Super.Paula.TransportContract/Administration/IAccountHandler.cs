using Super.Paula.Administration.Requests;
using Super.Paula.Administration.Responses;

namespace Super.Paula.Administration
{
    public interface IAccountHandler
    {
        ValueTask RegisterInspectorAsync(RegisterInspectorRequest request);
        ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request);
        ValueTask<SignInInspectorResponse> SignInInspectorAsync(SignInInspectorRequest request);
        ValueTask SignOutInspectorAsync();
        ValueTask ChangeSecretAsync(ChangeSecretRequest request);
        ValueTask<QueryAuthorizationsResponse> QueryAuthorizationsAsync();

        ValueTask<StartImpersonationResponse> StartImpersonationAsync(StartImpersonationRequest request);
        ValueTask StopImpersonationAsync();
        ValueTask RepairChiefInspectorAsync(RepairChiefInspectorRequest request);
        ValueTask<AssessChiefInspectorDefectivenessResponse> AssessChiefInspectorDefectivenessAsync(AssessChiefInspectorDefectivenessRequest request);
    }
}