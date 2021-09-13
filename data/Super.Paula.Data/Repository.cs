using Microsoft.EntityFrameworkCore;
using Super.Paula.Environment;
using Super.Paula.Mapping.PartitionKeyValueGenerators;

namespace Super.Paula
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly PaulaContext _repositoryContext;
        private readonly AppState _appState;
        private readonly IPartitionKeyValueGenerator<TEntity> _partitionKeyValueGenerator;

        public Repository(PaulaContext repositoryContext, AppState appState, IPartitionKeyValueGenerator<TEntity> partitionKeyValueGenerator)
        {
            _repositoryContext = repositoryContext;
            _appState = appState;
            _partitionKeyValueGenerator = partitionKeyValueGenerator;
        }

        public async ValueTask<TEntity> GetByIdAsync(object id)
            => await _repositoryContext.FindAsync<TEntity>(AdjustForPartitionKey(id))
               ?? throw new RepositoryException($"Entity with id ({id}) was not found");

        public ValueTask<TEntity?> GetByIdOrDefaultAsync(object id)
            => _repositoryContext.FindAsync<TEntity>(AdjustForPartitionKey(id));

        public async ValueTask<TEntity> GetByIdsAsync(params object[] ids)
            => await _repositoryContext.FindAsync<TEntity>(AdjustForPartitionKey(ids))
               ?? throw new RepositoryException($"Entity with ids ({string.Join(',', ids)}) was not found");

        public ValueTask<TEntity?> GetByIdsOrDefaultAsync(params object[] ids)
            => _repositoryContext.FindAsync<TEntity>(AdjustForPartitionKey(ids));

        public IQueryable<TEntity> GetQueryable()
            => _repositoryContext.Set<TEntity>().AsNoTracking();

        public IAsyncEnumerable<TEntity> GetAsyncEnumerable()
            => GetQueryable().AsAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query)
            => query.Invoke(GetQueryable()).AsAsyncEnumerable();

        public IQueryable<TEntity> GetPartitionQueryable(params object[] partitionKeyComponents)
        {
            var partitionKeyCompoenentsQueue = new Queue<object>(partitionKeyComponents);
            var partitionKey = _partitionKeyValueGenerator.Value(_appState, partitionKeyCompoenentsQueue);

            var query = _repositoryContext.Set<TEntity>().AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(partitionKey))
            {
                query = query.WithPartitionKey(partitionKey);
            }

            return query.AsNoTracking();
        }

        public IAsyncEnumerable<TEntity> GetPartitionAsyncEnumerable(params object[] partitionKeyComponents)
            => GetPartitionQueryable(partitionKeyComponents).AsAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetPartitionAsyncEnumerable<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query, params object[] partitionKeyComponents)
            => query.Invoke(GetPartitionQueryable(partitionKeyComponents)).AsAsyncEnumerable();


        public async ValueTask InsertAsync(TEntity entity)
        {
            _repositoryContext.Add(entity);
            await _repositoryContext.SaveChangesAsync();
        }

        public async ValueTask UpdateAsync(TEntity entity)
        {
            if (_repositoryContext.Entry(entity).State == EntityState.Detached)
            {
                var entityEntry = _repositoryContext.Add(entity);
                entityEntry.State = EntityState.Modified;
            } 

            await _repositoryContext.SaveChangesAsync();
        }

        public async ValueTask DeleteAsync(TEntity entity)
        {
            if (_repositoryContext.Entry(entity).State == EntityState.Detached)
            {
                var entityEntry = _repositoryContext.Add(entity);
                entityEntry.State = EntityState.Unchanged;
            }

            _repositoryContext.Remove(entity);
            await _repositoryContext.SaveChangesAsync();
        }

        private object[] AdjustForPartitionKey(params object[] ids)
        {
            var partitionKeyComponents = new Queue<object>(ids);

            var partitionKey = _partitionKeyValueGenerator
                .Value(_appState, partitionKeyComponents);

            return partitionKeyComponents
                .Prepend(partitionKey)
                .ToArray();
        }
    }
}