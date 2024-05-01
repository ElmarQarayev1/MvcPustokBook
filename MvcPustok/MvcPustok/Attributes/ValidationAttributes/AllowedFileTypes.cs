using System;
using System.ComponentModel.DataAnnotations;

namespace MvcPustok.Attributes.ValidationAttributes
{
    public class AllowedFileTypes : ValidationAttribute
    {
        private string[] _types;
        public AllowedFileTypes(params string[] types)
        {
            _types = types;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            IFormFile? file = value as IFormFile;

            if (file != null)
            {
                if (!_types.Contains(file.ContentType))
                {
                    string errorMessage = "File must be one of the types: " + String.Join(",", _types);
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}

