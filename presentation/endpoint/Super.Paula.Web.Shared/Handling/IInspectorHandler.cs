using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Shared.Handling
{
    public interface IInspectorHandler
    {
        ValueTask<InspectorResponse> GetAsync(string inspector);
        IAsyncEnumerable<InspectorResponse> GetAll();
        IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization);

        ValueTask<InspectorResponse> CreateAsync(InspectorRequest request);
        ValueTask ReplaceAsync(string inspector, InspectorRequest request);
        ValueTask DeleteAsync(string inspector);

        ValueTask ActivateAsync(string inspector);
        ValueTask DeactivateAsync(string inspector);

        ValueTask RefreshOrganizationAsync(string organization, RefreshOrganizationRequest request);
    }
}