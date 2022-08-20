using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public interface IOrganizationRequestHandler
{
    ValueTask<OrganizationResponse> GetAsync(string organization);
    IAsyncEnumerable<OrganizationResponse> GetAll();

    ValueTask<OrganizationResponse> CreateAsync(OrganizationRequest request);
    ValueTask ReplaceAsync(string organization, OrganizationRequest request);
    ValueTask DeleteAsync(string organization, string etag);

    ValueTask<ActivateOrganizationResponse> ActivateAsync(string organization, string etag);
    ValueTask<DeactivateOrganizationResponse> DeactivateAsync(string organization, string etag);
    ValueTask<OrganizationResponse> RegisterAsync(RegisterOrganizationRequest request);
    ValueTask<InitializeOrganizationResponse> InitializeAsync(string organization, InitializeOrganizationRequest request);

}