using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CSI_Miami.Data.CustomAttributes
{
    public class ValidateFutureDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            value = (DateTime)value;

            if (DateTime.Now.CompareTo(value) == -1)
            {
                return new ValidationResult("Value cannot be greater then DateTime.Now!");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
