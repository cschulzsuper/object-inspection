using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorRequestHandler
    {
        ValueTask<InspectorResponse> GetAsync(string inspector);
        ValueTask<InspectorResponse> GetCurrentAsync();

        IAsyncEnumerable<InspectorResponse> GetAll();
        IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization);
        IAsyncEnumerable<IdentityInspectorResponse> GetAllForIdentity(string identity);

        ValueTask<InspectorResponse> CreateAsync(InspectorRequest request);
        ValueTask ReplaceAsync(string inspector, InspectorRequest request);
        ValueTask DeleteAsync(string inspector, string etag);

        ValueTask<ActivateInspectorResponse> ActivateAsync(string inspector, string etag);
        ValueTask<DeactivateInspectorResponse> DeactivateAsync(string inspector, string etag);
    }
}