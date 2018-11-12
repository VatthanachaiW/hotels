using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotels.TestUtilities
{
    public class ModelValidateHelper
    {
        public static IList<ValidationResult> Validation(object model)
        {
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, result, true);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            return result;
        }
    }
}