using MoviesAPI.Entitties;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Validations
{
    public class FirstLetterUppercaseAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //there is an opyion to access the entire modul via validationContext.ObjectInstance like in this example, but this something we usually don't want to do
            //var genre = (Genre)validationContext.ObjectInstance;
            //genre.Age

            if (value == null|| string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var firstLetter = value.ToString()[0].ToString();
            if(firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("First letter should be uppercase");
            }

            return ValidationResult.Success;

        }
    }
}
