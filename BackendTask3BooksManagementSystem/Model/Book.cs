using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackendTasks.Model
{
    public class Book
    {
        
        public int id { get; set; }
        [Required(ErrorMessage = "Title Is requreid")]
        public string? title { get; set; }
        [Required(ErrorMessage = "Author Is requreid")]
        [StringLength(50,ErrorMessage = "Author name cannot exceed 50 characters.")]
        public string? author { get; set; }
        [Required(ErrorMessage = "Published Date Is requreid")]
        [DataType(DataType.Date)]
        public DateTime publishedDate { get; set; }
    }
}
