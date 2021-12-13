using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IOrganizationHandler : IOrganizationProvider
    {
        ValueTask<OrganizationResponse> CreateAsync(OrganizationRequest request);
        ValueTask ReplaceAsync(string organization, OrganizationRequest request);
        ValueTask DeleteAsync(string organization);
        ValueTask DeactivateAsync(string organization);
        ValueTask ActivateAsync(string organization);
    }
}