using Microsoft.EntityFrameworkCore;
using Super.Paula.Data.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Data;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly PaulaContext _repositoryContext;
    private readonly PaulaContextState _appState;
    private readonly IPartitionKeyValueGenerator<TEntity> _partitionKeyValueGenerator;

    public Repository(PaulaContext repositoryContext, PaulaContextState appState, IPartitionKeyValueGenerator<TEntity> partitionKeyValueGenerator)
    {
        _repositoryContext = repositoryContext;
        _appState = appState;
        _partitionKeyValueGenerator = partitionKeyValueGenerator;
    }

    public virtual async ValueTask<TEntity> GetByIdAsync(object id)
    {
        var entity = await _repositoryContext.FindAsync<TEntity>(AdjustForPartitionKey(id));

        if (entity == null)
        {
            throw new RepositoryException($"Entity with id ({id}) was not found.");
        }

        DetachEntity(entity);
        return entity;
    }

    public virtual async ValueTask<TEntity?> GetByIdOrDefaultAsync(object id)
    {
        var entity = await _repositoryContext.FindAsync<TEntity>(AdjustForPartitionKey(id));
        DetachEntity(entity);
        return entity;
    }

    public virtual async ValueTask<TEntity> GetByIdsAsync(params object[] ids)
    {
        var entity = await _repositoryContext.FindAsync<TEntity>(AdjustForPartitionKey(ids));

        if (entity == null)
        {
            throw new RepositoryException($"Entity with ids ({string.Join(',', ids)}) was not found."); ;
        }

        DetachEntity(entity);
        return entity;
    }

    public virtual TEntity? GetByIdsOrDefault(params object[] ids)
    {
        var entity = _repositoryContext.Find<TEntity>(AdjustForPartitionKey(ids));
        DetachEntity(entity);
        return entity;
    }

    public virtual async ValueTask<TEntity?> GetByIdsOrDefaultAsync(params object[] ids)
    {
        var entity = await _repositoryContext.FindAsync<TEntity>(AdjustForPartitionKey(ids));
        DetachEntity(entity);
        return entity;
    }

    public IQueryable<TEntity> GetQueryable()
    {
        var x = _repositoryContext.Model.ToDebugString();
        return _repositoryContext.Set<TEntity>().AsNoTracking();
    }

    public IQueryable<TEntity> GetQueryable(FormattableString query)
        => _repositoryContext.Set<TEntity>()
            .FromSqlRaw(
                query.Format,
                query.GetArguments()
                    .Where(x => x != null)
                    .ToArray()!)
            .AsNoTracking();

    public async IAsyncEnumerable<TEntity> GetAsyncEnumerable(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var enumerable = GetQueryable().AsAsyncEnumerable();

        await foreach (var entity in enumerable
            .WithCancellation(cancellationToken))
        {
            yield return entity;
        }
    }

    public async IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var enumerable = query.Invoke(GetQueryable()).AsAsyncEnumerable();

        await foreach (var entity in enumerable
            .WithCancellation(cancellationToken))
        {
            yield return entity;
        }
    }

    public IQueryable<TEntity> GetPartitionQueryable(params object[] partitionKeyComponents)
    {
        var partitionKeyComponentsQueue = new Queue<object>(partitionKeyComponents);
        var partitionKey = _partitionKeyValueGenerator.Value(_appState, partitionKeyComponentsQueue);

        var query = _repositoryContext.Set<TEntity>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(partitionKey))
        {
            query = query.WithPartitionKey(partitionKey);
        }

        return query.AsNoTracking();
    }

    public IQueryable<TEntity> GetPartitionQueryable(FormattableString expression, params object[] partitionKeyComponents)
    {
        var partitionKeyComponentsQueue = new Queue<object>(partitionKeyComponents);
        var partitionKey = _partitionKeyValueGenerator.Value(_appState, partitionKeyComponentsQueue);

        var query = _repositoryContext.Set<TEntity>()
            .FromSqlRaw(
                expression.Format,
                expression.GetArguments()
                    .Where(x => x != null)
                    .ToArray()!);

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

    public virtual async ValueTask InsertAsync(TEntity entity)
    {
        _repositoryContext.Add(entity);

        await _repositoryContext.SaveChangesAsync();

        // HINT: https://github.com/dotnet/efcore/issues/24828
        // Can not save changes multiple times safely 
        _repositoryContext.ChangeTracker.Clear();
    }

    public virtual async ValueTask UpdateAsync(TEntity entity)
    {
        if (_repositoryContext.Entry(entity).State == EntityState.Detached)
        {
            var entityEntry = _repositoryContext.Add(entity);

            // The value of shadow key property 'BusinessObjectInspectionAudit.PartitionKey' is unknown when attempting to save changes.
            // This is because shadow property values cannot be preserved when the entity is not being tracked.
            // Consider adding the property to the entity's .NET type. See https://aka.ms/efcore-docs-owned-collections for more information.

            var partitionKeyProperty = entityEntry.Metadata.GetPartitionKeyProperty();
            if (partitionKeyProperty != null)
            {
                var partitionKey = entityEntry.Properties.Single(x => x.Metadata == partitionKeyProperty);
                partitionKey.IsModified = true;
            }

            entityEntry.State = EntityState.Modified;

        }

        await _repositoryContext.SaveChangesAsync();

        // HINT: https://github.com/dotnet/efcore/issues/24828
        // Can not save changes multiple times safely 
        _repositoryContext.ChangeTracker.Clear();
    }

    public virtual async ValueTask DeleteAsync(TEntity entity)
    {
        if (_repositoryContext.Entry(entity).State == EntityState.Detached)
        {
            var entityEntry = _repositoryContext.Add(entity);

            // The value of shadow key property 'BusinessObjectInspectionAudit.PartitionKey' is unknown when attempting to save changes.
            // This is because shadow property values cannot be preserved when the entity is not being tracked.
            // Consider adding the property to the entity's .NET type. See https://aka.ms/efcore-docs-owned-collections for more information.

            var partitionKeyProperty = entityEntry.Metadata.GetPartitionKeyProperty();
            if (partitionKeyProperty != null)
            {
                var partitionKey = entityEntry.Properties.Single(x => x.Metadata == partitionKeyProperty);
                partitionKey.IsModified = true;
            }

            entityEntry.State = EntityState.Unchanged;
        }

        _repositoryContext.Remove(entity);

        await _repositoryContext.SaveChangesAsync();

        // HINT: https://github.com/dotnet/efcore/issues/24828
        // Can not save changes multiple times safely 
        _repositoryContext.ChangeTracker.Clear();
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

    private void DetachEntity(TEntity? entity)
    {
        if (entity != null)
        {
            _repositoryContext.Entry(entity).State = EntityState.Detached;
        }
    }

}