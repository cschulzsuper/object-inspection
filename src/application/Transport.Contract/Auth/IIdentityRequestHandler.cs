using Super.Paula.Application.Auth.Requests;
using Super.Paula.Application.Auth.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auth;

public interface IIdentityRequestHandler
{
    ValueTask<IdentityResponse> GetAsync(string identity);
    IAsyncEnumerable<IdentityResponse> GetAll();

    ValueTask<IdentityResponse> CreateAsync(IdentityRequest request);
    ValueTask ReplaceAsync(string identity, IdentityRequest request);
    ValueTask DeleteAsync(string identity, string etag);
}