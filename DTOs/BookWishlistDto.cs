using Bookstore.Models;

namespace Bookstore.DTOs
{
    public class BookWishlistDto
    {
        public BookDto Book { get; set; }
        public bool IsInWishlist { get; set; }
    }
}
