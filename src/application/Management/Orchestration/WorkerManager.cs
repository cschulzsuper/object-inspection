using Super.Paula.Data;
using Super.Paula.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration;

public class WorkerManager : IWorkerManager
{
    private readonly IRepository<Worker> _workerRepository;

    public WorkerManager(IRepository<Worker> workerRepository)
    {
        _workerRepository = workerRepository;
    }

    public async ValueTask<Worker> GetAsync(string worker)
    {
        EnsureGetable(worker);

        var entity = await _workerRepository.GetByIdsOrDefaultAsync(worker);
        if (entity == null)
        {
            throw new ManagementException($"Worker '{worker}' was not found.");
        }

        return entity;
    }

    public async ValueTask InsertAsync(Worker worker)
    {
        EnsureInsertable(worker);

        try
        {
            await _workerRepository.InsertAsync(worker);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not insert worker '{worker.UniqueName}'.", exception);
        }
    }

    public async ValueTask UpdateAsync(Worker worker)
    {
        EnsureUpdateable(worker);

        try
        {
            await _workerRepository.UpdateAsync(worker);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not update worker '{worker.UniqueName}'.", exception);
        }
    }

    public async ValueTask DeleteAsync(Worker worker)
    {
        EnsureDeletable(worker);

        try
        {
            await _workerRepository.DeleteAsync(worker);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not delete worker '{worker.UniqueName}'.", exception);
        }
    }

    public IQueryable<Worker> GetQueryable()
        => _workerRepository.GetQueryable();

    public IAsyncEnumerable<Worker> GetAsyncEnumerable()
        => _workerRepository.GetAsyncEnumerable();

    public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Worker>, IQueryable<TResult>> query)
        => _workerRepository.GetAsyncEnumerable(query);

    private static void EnsureGetable(string worker)
        => Validator.Ensure($"unique name '{worker}' of worker",
            WorkerValidator.UniqueNameIsNotEmpty(worker),
            WorkerValidator.UniqueNameHasKebabCase(worker),
            WorkerValidator.UniqueNameIsNotTooLong(worker),
            WorkerValidator.UniqueNameHasValidValue(worker));

    private static void EnsureInsertable(Worker worker)
        => Validator.Ensure($"worker with unique name '{worker.UniqueName}'",
            WorkerValidator.UniqueNameIsNotEmpty(worker.UniqueName),
            WorkerValidator.UniqueNameHasKebabCase(worker.UniqueName),
            WorkerValidator.UniqueNameIsNotTooLong(worker.UniqueName),
            WorkerValidator.UniqueNameHasValidValue(worker.UniqueName),
            WorkerValidator.IterationDelayIsInDayTimeRange(worker.IterationDelay));

    private static void EnsureUpdateable(Worker worker)
        => Validator.Ensure($"worker with unique name '{worker.UniqueName}'",
            WorkerValidator.UniqueNameIsNotEmpty(worker.UniqueName),
            WorkerValidator.UniqueNameHasKebabCase(worker.UniqueName),
            WorkerValidator.UniqueNameIsNotTooLong(worker.UniqueName),
            WorkerValidator.UniqueNameHasValidValue(worker.UniqueName),
            WorkerValidator.IterationDelayIsInDayTimeRange(worker.IterationDelay));

    private static void EnsureDeletable(Worker worker)
        => Validator.Ensure($"worker with unique name '{worker.UniqueName}'",
            WorkerValidator.UniqueNameIsNotEmpty(worker.UniqueName),
            WorkerValidator.UniqueNameHasKebabCase(worker.UniqueName),
            WorkerValidator.UniqueNameIsNotTooLong(worker.UniqueName),
            WorkerValidator.UniqueNameHasValidValue(worker.UniqueName));
}