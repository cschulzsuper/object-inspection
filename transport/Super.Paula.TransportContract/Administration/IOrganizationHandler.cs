using Super.Paula.Administration.Requests;
using Super.Paula.Administration.Responses;

namespace Super.Paula.Administration
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