using System.ComponentModel.DataAnnotations;

namespace Bookstore.DTOs
{
    public class SendResetCodeDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
