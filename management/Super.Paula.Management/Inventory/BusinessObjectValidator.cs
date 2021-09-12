using Super.Paula.Aggregates.Administration;
using Super.Paula.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Super.Paula.Aggregates.Guidlines;
using Super.Paula.Aggregates.Inventory;

namespace Super.Paula.Management.Inventory
{
    public static class BusinessObjectValidator
    {
        public static (Func<bool, bool>, Func<FormattableString>) BusinessObjectHasValue(string businessObject)
            => (_ => !string.IsNullOrWhiteSpace(businessObject),
                    () => $"Business object must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) BusinessObjectHasKebabCase(string businessObject)
            => (x => x && KebabCaseValidator.IsValid(businessObject),
                    () => $"Business object '{businessObject}' must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) BusinessObjectExists(string businessObject, IQueryable<BusinessObject> businessObjects)
            => (x => x && businessObjects.FirstOrDefault(x => x.UniqueName == businessObject) != null,
                    () => $"Business object '{businessObject}' does not exist");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasValue(BusinessObject businessObject)
            => (_ => !string.IsNullOrWhiteSpace(businessObject.UniqueName),
                    () => $"Unique name of business object must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasKebabCase(BusinessObject businessObject)
            => (x => x && KebabCaseValidator.IsValid(businessObject.UniqueName),
                    () => $"Unique name '{businessObject.UniqueName}' of business object must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameIsUnqiue(BusinessObject businessObject, IQueryable<BusinessObject> businessObjects)
            => (x => x && businessObjects.FirstOrDefault(x => x.UniqueName == businessObject.UniqueName) == null,
                    () => $"Unique name '{businessObject.UniqueName}' of business object already exists");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameExists(BusinessObject businessObject, IQueryable<BusinessObject> businessObjects)
            => (x => x && businessObjects.FirstOrDefault(x => x.UniqueName == businessObject.UniqueName) != null,
                    () => $"Unique name '{businessObject.UniqueName}' of business object does not exist");

        public static (Func<bool, bool>, Func<FormattableString>) DisplayNameHasValue(BusinessObject businessObject)
            => (_ => !string.IsNullOrWhiteSpace(businessObject.DisplayName),
                    () => $"Display name of business object '{businessObject.UniqueName}' must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) InspectorIsNotNull(BusinessObject businessObject)
            => (_ => businessObject.Inspector != null,
                    () => $"Inspector of business object '{businessObject.UniqueName}' can not be null");

        public static (Func<bool, bool>, Func<FormattableString>) InspectorHasKebabCase(BusinessObject businessObject)
            => (x => x && KebabCaseValidator.IsValid(businessObject.Inspector),
                    () => $"Inspector '{businessObject.Inspector}' of business object '{businessObject.UniqueName}' must be in kebab case");
    }
}
