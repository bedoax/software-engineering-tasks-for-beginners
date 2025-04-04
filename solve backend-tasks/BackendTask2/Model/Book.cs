using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTasks.Model
{
    public class Book
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

     
      
        public int AuthorID { get; set; }

        [Required(ErrorMessage = "Published Date is required")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        public Author Author { get; set; } // العلاقة مع الكلاس `Author`
    }
}
