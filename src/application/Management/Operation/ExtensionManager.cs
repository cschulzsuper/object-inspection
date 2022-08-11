using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Operation
{
    public class ExtensionManager : IExtensionManager
    {
        private readonly IRepository<Extension> _extensionRepository;

        public ExtensionManager(IRepository<Extension> extensionRepository)
        {
            _extensionRepository = extensionRepository;
        }

        public async ValueTask<Extension> GetAsync(string aggregateType)
        {
            EnsureGetable(aggregateType);

            var entity = await _extensionRepository.GetByIdsOrDefaultAsync(aggregateType);
            if (entity == null)
            {
                throw new ManagementException($"Extension for '{aggregateType}' was not found.");
            }

            return entity;
        }

        public async ValueTask InsertAsync(Extension extension)
        {
            EnsureInsertable(extension);

            try
            {
                await _extensionRepository.InsertAsync(extension);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert extension for '{extension.AggregateType}'.", exception);
            }
        }

        public async ValueTask UpdateAsync(Extension extension)
        {
            EnsureUpdateable(extension);

            try
            {
                await _extensionRepository.UpdateAsync(extension);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update extension for '{extension.AggregateType}'.", exception);
            }
        }

        public async ValueTask DeleteAsync(Extension extension)
        {
            EnsureDeletable(extension);

            try
            {
                await _extensionRepository.DeleteAsync(extension);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete extension for '{extension.AggregateType}'.", exception);
            }
        }

        public IQueryable<Extension> GetQueryable()
            => _extensionRepository.GetQueryable();

        public IAsyncEnumerable<Extension> GetAsyncEnumerable()
            => _extensionRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Extension>, IQueryable<TResult>> query)
            => _extensionRepository.GetAsyncEnumerable(query);

        private static void EnsureGetable(string extension)
            => Validator.Ensure($"aggregate type '{extension}' of extension",
                ExtensionValidator.AggregateTypeIsNotEmpty(extension),
                ExtensionValidator.AggregateTypeHasKebabCase(extension),
                ExtensionValidator.AggregateTypeIsNotTooLong(extension),
                ExtensionValidator.AggregateTypeHasValidValue(extension));

        private static void EnsureInsertable(Extension extension)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return ExtensionValidator.AggregateTypeIsNotEmpty(extension.AggregateType);
                yield return ExtensionValidator.AggregateTypeHasKebabCase(extension.AggregateType);
                yield return ExtensionValidator.AggregateTypeIsNotTooLong(extension.AggregateType);
                yield return ExtensionValidator.AggregateTypeHasValidValue(extension.AggregateType);

                yield return ExtensionValidator.FieldUniqueNamesAreUnqiue(extension.Fields);
                yield return ExtensionValidator.FieldDataNamesAreUnqiue(extension.Fields);

                foreach (var extensionField in extension.Fields)
                {
                    yield return ExtensionFieldValidator.DisplayNameIsNotEmpty(extensionField.DisplayName);
                    yield return ExtensionFieldValidator.DisplayNameIsNotTooLong(extensionField.DisplayName);

                    yield return ExtensionFieldValidator.UniqueNameIsNotEmpty(extensionField.UniqueName);
                    yield return ExtensionFieldValidator.UniqueNameHasKebabCase(extensionField.UniqueName);
                    yield return ExtensionFieldValidator.UniqueNameIsNotTooLong(extensionField.UniqueName);
                    yield return ExtensionFieldValidator.UniqueNameHasValidValue(extensionField.UniqueName);

                    yield return ExtensionFieldValidator.DataTypeIsNotEmpty(extensionField.DataType);
                    yield return ExtensionFieldValidator.DataTypeHasLowerCase(extensionField.DataType);
                    yield return ExtensionFieldValidator.DataTypeIsNotTooLong(extensionField.DataType);
                    yield return ExtensionFieldValidator.DataTypeHasValidValue(extensionField.DataType);

                    yield return ExtensionFieldValidator.DataNameIsNotEmpty(extensionField.DataName);
                    yield return ExtensionFieldValidator.DataNameHasCamelCase(extensionField.DataName);
                    yield return ExtensionFieldValidator.DataNameIsNotTooLong(extensionField.DataName);
                    yield return ExtensionFieldValidator.DataNameHasValidValue(extensionField.DataName);
                }
            }

            Validator.Ensure($"extension with aggregate type '{extension.AggregateType}'", Ensurences());
        }

        private static void EnsureUpdateable(Extension extension)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return ExtensionValidator.AggregateTypeIsNotEmpty(extension.AggregateType);
                yield return ExtensionValidator.AggregateTypeHasKebabCase(extension.AggregateType);
                yield return ExtensionValidator.AggregateTypeIsNotTooLong(extension.AggregateType);
                yield return ExtensionValidator.AggregateTypeHasValidValue(extension.AggregateType);

                yield return ExtensionValidator.FieldUniqueNamesAreUnqiue(extension.Fields);
                yield return ExtensionValidator.FieldDataNamesAreUnqiue(extension.Fields);

                foreach (var extensionField in extension.Fields)
                {
                    yield return ExtensionFieldValidator.DisplayNameIsNotEmpty(extensionField.DisplayName);
                    yield return ExtensionFieldValidator.DisplayNameIsNotTooLong(extensionField.DisplayName);

                    yield return ExtensionFieldValidator.UniqueNameIsNotEmpty(extensionField.UniqueName);
                    yield return ExtensionFieldValidator.UniqueNameHasKebabCase(extensionField.UniqueName);
                    yield return ExtensionFieldValidator.UniqueNameIsNotTooLong(extensionField.UniqueName);
                    yield return ExtensionFieldValidator.UniqueNameHasValidValue(extensionField.UniqueName);

                    yield return ExtensionFieldValidator.DataTypeIsNotEmpty(extensionField.DataType);
                    yield return ExtensionFieldValidator.DataTypeHasLowerCase(extensionField.DataType);
                    yield return ExtensionFieldValidator.DataTypeIsNotTooLong(extensionField.DataType);
                    yield return ExtensionFieldValidator.DataTypeHasValidValue(extensionField.DataType);

                    yield return ExtensionFieldValidator.DataNameIsNotEmpty(extensionField.DataName);
                    yield return ExtensionFieldValidator.DataNameHasCamelCase(extensionField.DataName);
                    yield return ExtensionFieldValidator.DataNameIsNotTooLong(extensionField.DataName);
                    yield return ExtensionFieldValidator.DataNameHasValidValue(extensionField.DataName);
                }
            }

            Validator.Ensure($"extension with aggregate type '{extension.AggregateType}'", Ensurences());
        }

        private static void EnsureDeletable(Extension extension)
            => Validator.Ensure($"extension with aggregate type '{extension.AggregateType}'",
                ExtensionValidator.AggregateTypeIsNotEmpty(extension.AggregateType),
                ExtensionValidator.AggregateTypeHasKebabCase(extension.AggregateType),
                ExtensionValidator.AggregateTypeIsNotTooLong(extension.AggregateType),
                ExtensionValidator.AggregateTypeHasValidValue(extension.AggregateType));
    }
}