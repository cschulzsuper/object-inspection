using System.Collections.Generic;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Application.Authentication.Requests;
using ChristianSchulz.ObjectInspection.Application.Authentication.Responses;

namespace ChristianSchulz.ObjectInspection.Application.Authentication;

public interface IIdentityRequestHandler
{
    ValueTask<IdentityResponse> GetAsync(string identity);
    IAsyncEnumerable<IdentityResponse> GetAll();

    ValueTask<IdentityResponse> CreateAsync(IdentityRequest request);
    ValueTask ReplaceAsync(string identity, IdentityRequest request);
    ValueTask DeleteAsync(string identity, string etag);
}