using Super.Paula.Administration.Requests;
using Super.Paula.Administration.Responses;

namespace Super.Paula.Administration
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