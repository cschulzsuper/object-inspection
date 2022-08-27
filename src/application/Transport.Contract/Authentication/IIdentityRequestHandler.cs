using System.Collections.Generic;
using System.Threading.Tasks;
using Super.Paula.Application.Authentication.Requests;
using Super.Paula.Application.Authentication.Responses;

namespace Super.Paula.Application.Authentication;

public interface IIdentityRequestHandler
{
    ValueTask<IdentityResponse> GetAsync(string identity);
    IAsyncEnumerable<IdentityResponse> GetAll();

    ValueTask<IdentityResponse> CreateAsync(IdentityRequest request);
    ValueTask ReplaceAsync(string identity, IdentityRequest request);
    ValueTask DeleteAsync(string identity, string etag);
}