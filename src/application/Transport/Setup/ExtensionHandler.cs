using Super.Paula.Application.Auditing.Responses;
using Super.Paula.Application.Setup.Requests;
using Super.Paula.Application.Setup.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Setup
{
    public class ExtensionHandler : IExtensionHandler
    {
        private readonly IExtensionManager _extensionManager;

        public ExtensionHandler(
            IExtensionManager extensionManager)
        {
            _extensionManager = extensionManager;
        }

        public async ValueTask<ExtensionResponse> GetAsync(string type)
        {
            var entity = await _extensionManager.GetAsync(type);

            return new ExtensionResponse
            {
                ETag = entity.ETag,
                Type = entity.Type,
                Fields = entity.Fields.ToResponse()
            };
        }

        public IAsyncEnumerable<ExtensionResponse> GetAll()
            => _extensionManager
                .GetAsyncEnumerable(queryable => queryable
                    .Select(entity => new ExtensionResponse
                    {
                        ETag = entity.ETag,
                        Type = entity.Type,
                        Fields = entity.Fields.ToResponse()
                    }));

        public async ValueTask<ExtensionResponse> CreateAsync(ExtensionRequest request)
        {
            var entity = new Extension
            {
                Type = request.Type
            };

            await _extensionManager.InsertAsync(entity);

            return new ExtensionResponse
            {
                ETag = entity.ETag,
                Type = entity.Type
            };
        }

        public async ValueTask DeleteAsync(string type, string etag)
        {
            var entity = await _extensionManager.GetAsync(type);

            entity.ETag = etag;

            await _extensionManager.DeleteAsync(entity);
        }

        public async ValueTask<ExtensionFieldCreateResponse> CreateFieldAsync(string type, ExtensionFieldRequest request)
        {
            var entity = await _extensionManager.GetAsync(type);
            
            var entityField = new ExtensionField
            {
                Name = request.Name,
                Type = request.Type
            };

            entity.ETag = request.ETag;
            entity.Fields.Add(entityField);

            await _extensionManager.UpdateAsync(entity);

            return new ExtensionFieldCreateResponse
            {
                ETag = entity.ETag
            };
        }

        public async ValueTask<ExtensionFieldDeleteResponse> DeleteFieldAsync(string type, string field, string etag)
        {
            var entity = await _extensionManager.GetAsync(type);

            var entityField = entity.Fields
                .Single(x => x.Name == field);

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