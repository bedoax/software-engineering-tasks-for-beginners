using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackendTasks5.Model
{
    public class Book
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Title Is requreid")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Author Is requreid")]
        [StringLength(50,ErrorMessage = "Author name cannot exceed 50 characters.")]
        public string? Author { get; set; }
        [Required(ErrorMessage = "Published Date Is requreid")]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
    }
}
