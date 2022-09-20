using ChristianSchulz.ObjectInspection.Application.Administration.Requests;
using ChristianSchulz.ObjectInspection.Application.Administration.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

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