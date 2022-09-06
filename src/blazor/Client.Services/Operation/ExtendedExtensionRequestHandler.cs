using Super.Paula.Application.Operation;
using Super.Paula.Application.Operation.Requests;
using Super.Paula.Application.Operation.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Super.Paula.Application.Operation.Exceptions;
using System.Collections.Immutable;

namespace Super.Paula.Client.Operation;

public class ExtendedExtensionRequestHandler : IExtensionRequestHandler
{
    private readonly IExtensionRequestHandler _extendedExtensionRequestHandle;

    public ExtendedExtensionRequestHandler(IExtensionRequestHandler extendedExtensionRequestHandle)
    {
        _extendedExtensionRequestHandle = extendedExtensionRequestHandle;
    }

    public ValueTask<ExtensionResponse> CreateAsync(ExtensionRequest request)
        => _extendedExtensionRequestHandle.CreateAsync(request);

    public ValueTask<ExtensionFieldCreateResponse> CreateFieldAsync(string aggregateType, ExtensionFieldRequest request)
        => _extendedExtensionRequestHandle.CreateFieldAsync(aggregateType, request);

    public ValueTask DeleteAsync(string aggregateType, string etag)
        => _extendedExtensionRequestHandle.DeleteAsync(aggregateType, etag);

    public ValueTask<ExtensionFieldDeleteResponse> DeleteFieldAsync(string aggregateType, string field, string etag)
        => _extendedExtensionRequestHandle.DeleteFieldAsync(aggregateType, field, etag);

    public IAsyncEnumerable<ExtensionResponse> GetAll()
        => _extendedExtensionRequestHandle.GetAll();

    public ValueTask<ExtensionResponse> GetAsync(string aggregateType)
    {
        try
        {
            return _extendedExtensionRequestHandle.GetAsync(aggregateType);
        }
        catch (ExtensionNotFoundException)
        {
            var response = new ExtensionResponse
            {
                AggregateType = aggregateType,
                ETag = string.Empty,
                Fields = ImmutableHashSet<ExtensionFieldResponse>.Empty
            };

            return ValueTask.FromResult(response);
        }
    }
}