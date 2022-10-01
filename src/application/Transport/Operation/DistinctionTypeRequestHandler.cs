using ChristianSchulz.ObjectInspection.Application.Operation.Requests;
using ChristianSchulz.ObjectInspection.Application.Operation.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class DistinctionTypeRequestHandler : IDistinctionTypeRequestHandler
{
    private readonly IDistinctionTypeManager _extensionManager;

    public DistinctionTypeRequestHandler(
        IDistinctionTypeManager extensionManager)
    {
        _extensionManager = extensionManager;
    }

    public async ValueTask<DistinctionTypeResponse> GetAsync(string aggregateType)
    {
        var entity = await _extensionManager.GetAsync(aggregateType);

        return new DistinctionTypeResponse
        {
            ETag = entity.ETag,
            UniqueName = entity.UniqueName,
            Fields = entity.Fields.ToResponse()
        };
    }

    public IAsyncEnumerable<DistinctionTypeResponse> GetAll()
        => _extensionManager
            .GetAsyncEnumerable(queryable => queryable
                .Select(entity => new DistinctionTypeResponse
                {
                    ETag = entity.ETag,
                    UniqueName = entity.UniqueName,
                    Fields = entity.Fields.ToResponse()
                }));

    public async ValueTask<DistinctionTypeResponse> CreateAsync(DistinctionTypeRequest request)
    {
        var entity = new DistinctionType
        {
            UniqueName = request.UniqueName
        };

        await _extensionManager.InsertAsync(entity);

        return new DistinctionTypeResponse
        {
            ETag = entity.ETag,
            UniqueName = entity.UniqueName
        };
    }

    public async ValueTask DeleteAsync(string type, string etag)
    {
        var entity = await _extensionManager.GetAsync(type);

        entity.ETag = etag;

        await _extensionManager.DeleteAsync(entity);
    }

    public async ValueTask<DistinctionTypeFieldCreateResponse> CreateFieldAsync(string aggregateType, DistinctionTypeFieldRequest request)
    {
        var entity = await _extensionManager.GetAsync(aggregateType);

        var entityField = new DistinctionTypeField
        {
            ExtensionField = request.ExtensionField
        };

        entity.ETag = request.ETag;
        entity.Fields.Add(entityField);

        await _extensionManager.UpdateAsync(entity);

        return new DistinctionTypeFieldCreateResponse
        {
            ETag = entity.ETag
        };
    }

    public async ValueTask<DistinctionTypeFieldDeleteResponse> DeleteFieldAsync(string aggregateType, string field, string etag)
    {
        var entity = await _extensionManager.GetAsync(aggregateType);

        var entityField = entity.Fields
            .Single(x => x.ExtensionField == field);

        entity.ETag = etag;
        entity.Fields.Remove(entityField);

        await _extensionManager.UpdateAsync(entity);

        return new DistinctionTypeFieldDeleteResponse
        {
            ETag = entity.ETag
        };
    }
}