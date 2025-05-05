using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public DateTime PublishingDate { get; set; }
        public string PublishingHouse { get; set; }
        public byte[]? BookImage { get; set; }
        [DataType(DataType.Upload)]
        [DisplayName("Imagine")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<AuthorBook> AuthorBooks { get; set; }
        public ICollection<GenreBook> GenreBooks { get; set; }
        public ICollection<WishlistBook> WishlistBooks { get; set; }
    }
}
