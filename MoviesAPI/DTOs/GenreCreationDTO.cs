using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class GenreCreationDTO
    {
        [Required(ErrorMessage = "The field eith name {0} is required")]
        [StringLength(50)]
        [FirstLetterUppercase] //in this attribute we call FirstLetterUppercaseAttribute class without the "Attribute" part
        public string Name { get; set; }
    }
}
