using System.ComponentModel.DataAnnotations;

namespace BackendTask5.Model
{
    public class Role
    {

        public int Id { get; set; }
        [Required]
        public string roleTitle { get; set; }
    }
}
