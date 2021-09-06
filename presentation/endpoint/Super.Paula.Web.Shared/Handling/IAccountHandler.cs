using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Shared.Handling
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