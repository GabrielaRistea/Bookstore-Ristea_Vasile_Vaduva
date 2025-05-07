using Bookstore.Models;

namespace Bookstore.DTOs
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public User User { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
    }

}
