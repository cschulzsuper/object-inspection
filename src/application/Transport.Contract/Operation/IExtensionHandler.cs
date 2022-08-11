using Super.Paula.Application.Operation.Requests;
using Super.Paula.Application.Operation.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Operation
{
    public interface IExtensionHandler
    {
        ValueTask<ExtensionResponse> CreateAsync(ExtensionRequest request);
        ValueTask DeleteAsync(string aggregateType, string etag);
        IAsyncEnumerable<ExtensionResponse> GetAll();
        ValueTask<ExtensionResponse> GetAsync(string aggregateType);

        ValueTask<ExtensionFieldCreateResponse> CreateFieldAsync(string aggregateType, ExtensionFieldRequest request);
        ValueTask<ExtensionFieldDeleteResponse> DeleteFieldAsync(string aggregateType, string field, string etag);
    }
}