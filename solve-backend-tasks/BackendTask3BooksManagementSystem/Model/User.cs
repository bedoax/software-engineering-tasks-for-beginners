using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTask3.Model
{
    public class User
    {
        [Key]
        [Column("username")]
        [Required]
        public string? username { get; set; }
        [Column("password")]
        [Required]
        public string? password { get; set; }
    }
}
