using System.ComponentModel.DataAnnotations;

public class Register
{
    [Required]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
    public string username { get; set; }

    [Required]
    public string password { get; set; }
}
