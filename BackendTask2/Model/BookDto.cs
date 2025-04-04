using System.ComponentModel.DataAnnotations;

namespace BackendTask2.Model
{
    public class BookDto
    {
        public string? Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
    }
}
