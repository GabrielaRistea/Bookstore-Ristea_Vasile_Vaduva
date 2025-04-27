using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Bookstore.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public Address? Address { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Wishlist Wishlist { get; set; }
        [StringLength(4)]
        public string? CodeResetPassword { get; set; }
        public DateTime? TimeCodeExpires { get; set; }
    }
}
