using System.ComponentModel.DataAnnotations;

namespace VolleyLeague.Client.Blazor.Shared.Helpers
{
    public class ValidateEachItemAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            //List<ValidationResult> validationResults = new List<ValidationResult>();
            //var list = value as IEnumerable;
            //if (list == null) return true;

            //var isValid = true;
            //foreach (var item in list)
            //{
            //    var validationContext = new ValidationContext(item);
            //    var isItemValid = Validator.TryValidateObject(item, validationContext, validationResults, true);
            //    isValid &= isItemValid;
            //}
            //ErrorMessage = string.Join(" ", validationResults.Select(r => r.ErrorMessage));
            return true;
        }
    }
}
