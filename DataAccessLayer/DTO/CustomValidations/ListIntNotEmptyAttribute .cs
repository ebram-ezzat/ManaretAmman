using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.CustomValidations
{
    public class ListIntNotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList<int>;
            if (list == null || list.Count == 0)
            {
                return new ValidationResult(ErrorMessage ?? "The list must contain at least one item.");
            }
            return ValidationResult.Success;
        }
    }
}
