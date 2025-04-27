using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class WishlistBook
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Book))]
        public int IdBook { get; set; }
        public Book Book { get; set; }
        [ForeignKey(nameof(Wishlist))]
        public int IdWishlist { get; set; }
        public Wishlist Wishlist { get; set; }
    }
}
