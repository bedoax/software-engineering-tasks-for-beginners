using System.ComponentModel.DataAnnotations;

namespace BackendTask2.Model
{
    public class AuthorDto
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Author Name Is requreid")]
        [StringLength(50, ErrorMessage = "Author name cannot exceed 50 characters.")]
        public string? Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
