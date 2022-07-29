using Super.Paula.Application.Setup.Requests;
using Super.Paula.Application.Setup.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Setup
{
    public interface IExtensionHandler
    {
        ValueTask<ExtensionResponse> CreateAsync(ExtensionRequest request);
        ValueTask DeleteAsync(string type, string etag);
        IAsyncEnumerable<ExtensionResponse> GetAll();
        ValueTask<ExtensionResponse> GetAsync(string type);

        ValueTask<ExtensionFieldCreateResponse> CreateFieldAsync(string type, ExtensionFieldRequest request);
        ValueTask<ExtensionFieldDeleteResponse> DeleteFieldAsync(string type, string field, string etag);
    }
}