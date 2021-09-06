using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Shared.Handling
{
    public interface IOrganizationHandler
    {
        ValueTask<OrganizationResponse> GetAsync(string organization);
        IAsyncEnumerable<OrganizationResponse> GetAll();
        ValueTask<OrganizationResponse> CreateAsync(OrganizationRequest request);
        ValueTask ReplaceAsync(string organization, OrganizationRequest request);
        ValueTask DeleteAsync(string organization);
        ValueTask DeactivateAsync(string organization);
        ValueTask ActivateAsync(string organization);
    }
}