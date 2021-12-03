using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class  IdentityManager : IIdentityManager
    {
        private readonly IRepository<Identity> _identityRepository;

        public IdentityManager(IRepository<Identity> identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async ValueTask<Identity> GetAsync(string identity)
        {
            EnsureGetable(identity);

            var entity = await _identityRepository.GetByIdsOrDefaultAsync(identity);
            if (entity == null)
            {
                throw new ManagementException($"Identity '{identity}' was not found");
            }

            return entity;
        }

        public IQueryable<Identity> GetQueryable()
            => _identityRepository.GetQueryable();

        public IAsyncEnumerable<Identity> GetAsyncEnumerable()
            => _identityRepository.GetPartitionAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Identity>, IQueryable<TResult>> query)
            => _identityRepository.GetPartitionAsyncEnumerable(query);

        public async ValueTask InsertAsync(Identity identity)
        {
            EnsureInsertable(identity);

            try
            {
                await _identityRepository.InsertAsync(identity);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert identity '{identity.UniqueName}'", exception);
            }
        }

        public async ValueTask UpdateAsync(Identity identity)
        {
            EnsureUpdateable(identity);

            try
            {
                await _identityRepository.UpdateAsync(identity);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update identity '{identity.UniqueName}'", exception);
            }
        }

        public async ValueTask DeleteAsync(Identity identity)
        {
            EnsureDeleteable(identity);

            try
            {
                await _identityRepository.DeleteAsync(identity);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete identity '{identity.UniqueName}'", exception);
            }
        }

        private static void EnsureGetable(string identity)
            => Validator.Ensure($"unique name '{identity}' of identity",
                IdentityValidator.UniqueNameIsNotEmpty(identity),
                IdentityValidator.UniqueNameHasKebabCase(identity),
                IdentityValidator.UniqueNameIsNotTooLong(identity));

        private static void EnsureInsertable(Identity identity)
            => Validator.Ensure($"identity with unique name '{identity.UniqueName}'",
                IdentityValidator.UniqueNameIsNotEmpty(identity.UniqueName),
                IdentityValidator.UniqueNameHasKebabCase(identity.UniqueName),
                IdentityValidator.UniqueNameIsNotTooLong(identity.UniqueName),
                IdentityValidator.MailAddressIsNotNull(identity.MailAddress),
                IdentityValidator.MailAddressIsMailAddress(identity.MailAddress),
                IdentityValidator.MailAddressIsNotTooLong(identity.MailAddress),
                IdentityValidator.SecretHasValue(identity.Secret),
                IdentityValidator.SecretIsNotTooLong(identity.Secret));

        private static void EnsureUpdateable(Identity identity)
            => Validator.Ensure($"identity with unique name '{identity.UniqueName}'",
                IdentityValidator.UniqueNameIsNotEmpty(identity.UniqueName),
                IdentityValidator.UniqueNameHasKebabCase(identity.UniqueName),
                IdentityValidator.UniqueNameIsNotTooLong(identity.UniqueName),
                IdentityValidator.MailAddressIsNotNull(identity.MailAddress),
                IdentityValidator.MailAddressIsMailAddress(identity.MailAddress),
                IdentityValidator.MailAddressIsNotTooLong(identity.MailAddress),
                IdentityValidator.SecretHasValue(identity.Secret),
                IdentityValidator.SecretIsNotTooLong(identity.Secret));

        private static void EnsureDeleteable(Identity identity)
            => Validator.Ensure($"identity with unique name '{identity.UniqueName}'",
                IdentityValidator.UniqueNameIsNotEmpty(identity.UniqueName),
                IdentityValidator.UniqueNameHasKebabCase(identity.UniqueName),
                IdentityValidator.UniqueNameIsNotTooLong(identity.UniqueName));
    }
}