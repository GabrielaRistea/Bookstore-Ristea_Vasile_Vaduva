using Bookstore.Models;

namespace Bookstore.DTOs
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public User User { get; set; }
    }

}
