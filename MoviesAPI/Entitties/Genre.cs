using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entitties
{
    public class Genre
    {
        //for custom validation : Model Validation - we need to implement IValidatableObject
        public int Id { get; set; }
        [Required(ErrorMessage ="The field eith name {0} is required")]
        [StringLength(50)]
        [FirstLetterUppercase] //in this attribute we call FirstLetterUppercaseAttribute class without the "Attribute" part
        public string Name { get; set; }

    }
}
