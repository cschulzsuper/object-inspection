using Super.Paula.Application.Operation;
using Super.Paula.Data.Mappings;
using System.Threading.Tasks;

namespace Super.Paula.Data
{
    public sealed class ExtensionRepository : Repository<Extension>
    {
        private readonly ExtensionCache _extensionCache;

        public ExtensionRepository(
            PaulaContexts paulaContexts, 
            PaulaContextState appState, 
            IPartitionKeyValueGenerator<Extension> partitionKeyValueGenerator,
            ExtensionCache extensionCache)

            : base(paulaContexts.Operation, appState, partitionKeyValueGenerator)
        {
            _extensionCache = extensionCache;
        }

        public override async ValueTask InsertAsync(Extension entity)
        {
            await base.InsertAsync(entity);

            _extensionCache[entity.AggregateType] = null;
        }

        public override async ValueTask UpdateAsync(Extension entity)
        {
            await base.UpdateAsync(entity);

            _extensionCache[entity.AggregateType] = null;
        }

        public override async ValueTask DeleteAsync(Extension entity)
        {
            await base.DeleteAsync(entity);

            _extensionCache[entity.AggregateType] = null;
        }
    }
}