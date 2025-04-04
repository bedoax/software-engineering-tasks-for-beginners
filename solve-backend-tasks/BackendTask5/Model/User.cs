using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTask5.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Column("username")]
        [Required]
        public string? Username { get; set; }
        [Column("password")]
        [Required]
        public string? password { get; set; }
        public int RoleId { get; set; }
        public Role? role { get; set; }
    }
}
