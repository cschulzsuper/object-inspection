using Super.Paula.Aggregates.BusinessObjects;
using Super.Paula.Shared.ErrorHandling;

namespace Super.Paula.Data
{
    public interface IRepository<TEntity> 
        where TEntity : class
    {
        [ErrorMessage("Could not query entity by id")]
        ValueTask<TEntity> GetByIdAsync(object id);

        [ErrorMessage("Could not query entity by id")]
        ValueTask<TEntity?> GetByIdOrDefaultAsync(object id);

        [ErrorMessage("Could not query entity by ids")]
        ValueTask<TEntity> GetByIdsAsync(params object[] ids);

        [ErrorMessage("Could not query entity by ids")]
        ValueTask<TEntity?> GetByIdsOrDefaultAsync(params object[] ids);

        [ErrorMessage("Could not query entity list")]
        IQueryable<TEntity> GetQueryable();

        [ErrorMessage("Could not query entity list")]
        IAsyncEnumerable<TEntity> GetAsyncEnumerable();

        [ErrorMessage("Could not query entity list")]
        IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query);

        [ErrorMessage("Could not query entity list")]
        IQueryable<TEntity> GetPartitionQueryable(params object[] partitionKeyComponents);

        [ErrorMessage("Could not query entity list")]
        IAsyncEnumerable<TEntity> GetPartitionAsyncEnumerable(params object[] partitionKeyComponents);

        [ErrorMessage("Could not query entity list")]
        IAsyncEnumerable<TResult> GetPartitionAsyncEnumerable<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query, params object[] partitionKeyComponents);

        [ErrorMessage("Could not insert entity")]
        ValueTask InsertAsync(TEntity entity);

        [ErrorMessage("Could not update entity")]
        ValueTask UpdateAsync(TEntity entity);

        [ErrorMessage("Could not delete entity")]
        ValueTask DeleteAsync(TEntity entity);
    }
}