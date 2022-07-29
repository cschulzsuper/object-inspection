using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Setup
{
    public class ExtensionManager : IExtensionManager
    {
        private readonly IRepository<Extension> _extensionRepository;

        public ExtensionManager(IRepository<Extension> extensionRepository)
        {
            _extensionRepository = extensionRepository;
        }

        public async ValueTask<Extension> GetAsync(string type)
        {
            EnsureGetable(type);

            var entity = await _extensionRepository.GetByIdsOrDefaultAsync(type);
            if (entity == null)
            {
                throw new ManagementException($"Extension for '{type}' was not found.");
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
                throw new ManagementException($"Could not insert extension for '{extension.Type}'.", exception);
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
                throw new ManagementException($"Could not update extension for '{extension.Type}'.", exception);
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
                throw new ManagementException($"Could not delete extension for '{extension.Type}'.", exception);
            }
        }

        public IQueryable<Extension> GetQueryable()
            => _extensionRepository.GetQueryable();

        public IAsyncEnumerable<Extension> GetAsyncEnumerable()
            => _extensionRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Extension>, IQueryable<TResult>> query)
            => _extensionRepository.GetAsyncEnumerable(query);

        private static void EnsureGetable(string extension)
            => Validator.Ensure($"type '{extension}' of extension",
                ExtensionValidator.TypeIsNotEmpty(extension),
                ExtensionValidator.TypeHasKebabCase(extension),
                ExtensionValidator.TypeIsNotTooLong(extension),
                ExtensionValidator.TypeHasValidValue(extension));

        private static void EnsureInsertable(Extension extension)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return ExtensionValidator.TypeIsNotEmpty(extension.Type);
                yield return ExtensionValidator.TypeHasKebabCase(extension.Type);
                yield return ExtensionValidator.TypeIsNotTooLong(extension.Type);
                yield return ExtensionValidator.TypeHasValidValue(extension.Type);

                yield return ExtensionValidator.FieldsUnique(extension.Fields);

                foreach (var extensionField in extension.Fields)
                {
                    yield return ExtensionFieldValidator.TypeIsNotEmpty(extensionField.Type);
                    yield return ExtensionFieldValidator.TypeHasLowerCase(extensionField.Type);
                    yield return ExtensionFieldValidator.TypeIsNotTooLong(extensionField.Type);
                    yield return ExtensionFieldValidator.TypeHasValidValue(extensionField.Type);

                    yield return ExtensionFieldValidator.NameIsNotEmpty(extensionField.Name);
                    yield return ExtensionFieldValidator.NameHasCamelCase(extensionField.Name);
                    yield return ExtensionFieldValidator.NameIsNotTooLong(extensionField.Name);
                    yield return ExtensionFieldValidator.NameHasValidValue(extensionField.Name);
                }
            }

            Validator.Ensure($"extension with type '{extension.Type}'", Ensurences());
        }

        private static void EnsureUpdateable(Extension extension)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return ExtensionValidator.TypeIsNotEmpty(extension.Type);
                yield return ExtensionValidator.TypeHasKebabCase(extension.Type);
                yield return ExtensionValidator.TypeIsNotTooLong(extension.Type);
                yield return ExtensionValidator.TypeHasValidValue(extension.Type);

                yield return ExtensionValidator.FieldsUnique(extension.Fields);

                foreach (var extensionField in extension.Fields)
                {
                    yield return ExtensionFieldValidator.TypeIsNotEmpty(extensionField.Type);
                    yield return ExtensionFieldValidator.TypeHasLowerCase(extensionField.Type);
                    yield return ExtensionFieldValidator.TypeIsNotTooLong(extensionField.Type);
                    yield return ExtensionFieldValidator.TypeHasValidValue(extensionField.Type);

                    yield return ExtensionFieldValidator.NameIsNotEmpty(extensionField.Name);
                    yield return ExtensionFieldValidator.NameHasCamelCase(extensionField.Name);
                    yield return ExtensionFieldValidator.NameIsNotTooLong(extensionField.Name);
                    yield return ExtensionFieldValidator.NameHasValidValue(extensionField.Name);
                }
            }

            Validator.Ensure($"extension with type '{extension.Type}'", Ensurences());
        }

        private static void EnsureDeletable(Extension extension)
            => Validator.Ensure($"extension with type '{extension.Type}'",
                ExtensionValidator.TypeIsNotEmpty(extension.Type),
                ExtensionValidator.TypeHasKebabCase(extension.Type),
                ExtensionValidator.TypeIsNotTooLong(extension.Type),
                ExtensionValidator.TypeHasValidValue(extension.Type));
    }
}