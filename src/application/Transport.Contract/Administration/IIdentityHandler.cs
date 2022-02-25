using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IIdentityHandler
    {
        ValueTask<IdentityResponse> GetAsync(string identity);
        IAsyncEnumerable<IdentityResponse> GetAll();

        ValueTask<IdentityResponse> CreateAsync(IdentityRequest request);
        ValueTask ReplaceAsync(string identity, IdentityRequest request);
        ValueTask DeleteAsync(string identity, string etag);

        ValueTask<ResetIdentityResponse> ResetAsync(string identity, string etag);
    }
}