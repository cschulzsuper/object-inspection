using Super.Paula.Application.Auditing.Responses;
using Super.Paula.Application.Operation.Requests;
using Super.Paula.Application.Operation.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Operation
{
    public class ExtensionHandler : IExtensionHandler
    {
        private readonly IExtensionManager _extensionManager;

        public ExtensionHandler(
            IExtensionManager extensionManager)
        {
            _extensionManager = extensionManager;
        }

        public async ValueTask<ExtensionResponse> GetAsync(string aggregateType)
        {
            var entity = await _extensionManager.GetAsync(aggregateType);

            return new ExtensionResponse
            {
                ETag = entity.ETag,
                AggregateType = entity.AggregateType,
                Fields = entity.Fields.ToResponse()
            };
        }

        public IAsyncEnumerable<ExtensionResponse> GetAll()
            => _extensionManager
                .GetAsyncEnumerable(queryable => queryable
                    .Select(entity => new ExtensionResponse
                    {
                        ETag = entity.ETag,
                        AggregateType = entity.AggregateType,
                        Fields = entity.Fields.ToResponse()
                    }));

        public async ValueTask<ExtensionResponse> CreateAsync(ExtensionRequest request)
        {
            var entity = new Extension
            {
                AggregateType = request.AggregateType
            };

            await _extensionManager.InsertAsync(entity);

            return new ExtensionResponse
            {
                ETag = entity.ETag,
                AggregateType = entity.AggregateType
            };
        }

        public async ValueTask DeleteAsync(string type, string etag)
        {
            var entity = await _extensionManager.GetAsync(type);

            entity.ETag = etag;

            await _extensionManager.DeleteAsync(entity);
        }

        public async ValueTask<ExtensionFieldCreateResponse> CreateFieldAsync(string aggregateType, ExtensionFieldRequest request)
        {
            var entity = await _extensionManager.GetAsync(aggregateType);

            var entityField = new ExtensionField
            {
                UniqueName = request.UniqueName,
                DisplayName = request.DisplayName,
                DataType = request.DataType,
                DataName = request.DataName
            };

            entity.ETag = request.ETag;
            entity.Fields.Add(entityField);

            await _extensionManager.UpdateAsync(entity);

            return new ExtensionFieldCreateResponse
            {
                ETag = entity.ETag
            };
        }

        public async ValueTask<ExtensionFieldDeleteResponse> DeleteFieldAsync(string aggregateType, string field, string etag)
        {
            var entity = await _extensionManager.GetAsync(aggregateType);

            var entityField = entity.Fields
                .Single(x => x.UniqueName == field);

            entity.ETag = etag;
            entity.Fields.Remove(entityField);

            await _extensionManager.UpdateAsync(entity);

            return new ExtensionFieldDeleteResponse
            {
                ETag = entity.ETag
            };
        }
    }
}