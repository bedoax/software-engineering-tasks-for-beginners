using System.ComponentModel.DataAnnotations;

namespace BackendTasks.Model
{
    public class Author
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Author Name Is requreid")]
        [StringLength(50, ErrorMessage = "Author name cannot exceed 50 characters.")]
        public string? Name { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
