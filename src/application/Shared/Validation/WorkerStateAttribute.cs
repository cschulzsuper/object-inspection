using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class WorkerStateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return WorkerStateValidator.IsValid(value);
        }
    }
}
