using ChristianSchulz.ObjectInspection.Application.Operation.Requests;
using ChristianSchulz.ObjectInspection.Application.Operation.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IExtensionRequestHandler
{
    ValueTask<ExtensionResponse> CreateAsync(ExtensionRequest request);
    ValueTask DeleteAsync(string aggregateType, string etag);
    IAsyncEnumerable<ExtensionResponse> GetAll();
    ValueTask<ExtensionResponse> GetAsync(string aggregateType);

    ValueTask<ExtensionFieldCreateResponse> CreateFieldAsync(string aggregateType, ExtensionFieldRequest request);
    ValueTask<ExtensionFieldDeleteResponse> DeleteFieldAsync(string aggregateType, string field, string etag);
}