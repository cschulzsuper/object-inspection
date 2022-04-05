using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public class ContinuationManager : IContinuationManager
    {
        private readonly IRepository<Continuation> _continuationRepository;

        public ContinuationManager(IRepository<Continuation> continuationRepository)
        {
            _continuationRepository = continuationRepository;
        }

        public async ValueTask<Continuation> GetAsync(string id)
        {
            EnsureGetable(id);

            var entity = await _continuationRepository.GetByIdsOrDefaultAsync(id);
            if (entity == null)
            {
                throw new ManagementException($"Continuation '{id}' was not found.");
            }

            return entity;
        }

        public async ValueTask InsertAsync(Continuation continuation)
        {
            EnsureInsertable(continuation);

            try
            {
                await _continuationRepository.InsertAsync(continuation);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert continuation '{continuation.Name}'.", exception);
            }
        }

        public async ValueTask UpdateAsync(Continuation continuation)
        {
            EnsureUpdateable(continuation);

            try
            {
                await _continuationRepository.UpdateAsync(continuation);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update continuation '{continuation.Id}'.", exception);
            }
        }

        public async ValueTask DeleteAsync(Continuation continuation)
        {
            EnsureDeletable(continuation);

            try
            {
                await _continuationRepository.DeleteAsync(continuation);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete continuation '{continuation.Id}'.", exception);
            }
        }

        public IQueryable<Continuation> GetQueryable()
            => _continuationRepository.GetQueryable();

        public IAsyncEnumerable<Continuation> GetAsyncEnumerable()
            => _continuationRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Continuation>, IQueryable<TResult>> query)
            => _continuationRepository.GetAsyncEnumerable(query);

        private static void EnsureGetable(string id)
            => Validator.Ensure($"id '{id}' of continuation",
                ContinuationValidator.IdIsNotEmpty(id),
                ContinuationValidator.IdIsGuid(id));

        private static void EnsureInsertable(Continuation continuation)
            => Validator.Ensure($"continuation with unique name '{continuation.Name}'",
                ContinuationValidator.IdIsNotEmpty(continuation.Id),
                ContinuationValidator.IdIsGuid(continuation.Id),
                ContinuationValidator.NameIsNotEmpty(continuation.Name),
                ContinuationValidator.NameHasKebabCase(continuation.Name),
                ContinuationValidator.NameIsNotTooLong(continuation.Name),
                ContinuationValidator.NameHasValidValue(continuation.Name),
                ContinuationValidator.StateIsNotNull(continuation.State),
                ContinuationValidator.StateHasValidValue(continuation.State),
                ContinuationValidator.OperationIdIsNotEmpty(continuation.OperationId),
                ContinuationValidator.DataIsNotNull(continuation.Data),
                ContinuationValidator.DataIsNotTooLong(continuation.Data),
                ContinuationValidator.UserIsNotNull(continuation.User),
                ContinuationValidator.UserIsNotTooLong(continuation.User),
                ContinuationValidator.CreationDateIsPositive(continuation.CreationDate),
                ContinuationValidator.CreationTimeIsInDayTimeRange(continuation.CreationTime));

        private static void EnsureUpdateable(Continuation continuation)
            => Validator.Ensure($"continuation with id '{continuation.Id}'",
                ContinuationValidator.IdIsNotEmpty(continuation.Id),
                ContinuationValidator.IdIsGuid(continuation.Id),
                ContinuationValidator.NameIsNotEmpty(continuation.Name),
                ContinuationValidator.NameHasKebabCase(continuation.Name),
                ContinuationValidator.NameIsNotTooLong(continuation.Name),
                ContinuationValidator.NameHasValidValue(continuation.Name),
                ContinuationValidator.StateIsNotNull(continuation.State),
                ContinuationValidator.StateHasValidValue(continuation.State),
                ContinuationValidator.OperationIdIsNotEmpty(continuation.OperationId),
                ContinuationValidator.DataIsNotNull(continuation.Data),
                ContinuationValidator.DataIsNotTooLong(continuation.Data),
                ContinuationValidator.UserIsNotNull(continuation.User),
                ContinuationValidator.UserIsNotTooLong(continuation.User),
                ContinuationValidator.CreationDateIsPositive(continuation.CreationDate),
                ContinuationValidator.CreationTimeIsInDayTimeRange(continuation.CreationTime));

        private static void EnsureDeletable(Continuation continuation)
            => Validator.Ensure($"continuation with id '{continuation.Id}'",
                ContinuationValidator.IdIsNotEmpty(continuation.Id),
                ContinuationValidator.IdIsGuid(continuation.Id));
    }
}