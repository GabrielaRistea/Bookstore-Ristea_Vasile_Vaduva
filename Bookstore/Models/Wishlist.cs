using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }

        [ForeignKey(nameof(User))]
        public int IdUser { get; set; }
        public User User { get; set; }
        public ICollection<WishlistBook> WishlistBooks { get; set; }
    }
}
