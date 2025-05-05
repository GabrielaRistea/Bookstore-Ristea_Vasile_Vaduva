using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bookstore.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime PublishingDate { get; set; }
        [Required]
        public string PublishingHouse { get; set; }
        public byte[] BookImage { get; set; }
        [DataType(DataType.Upload)]
        [DisplayName("Image")]
        [NotMapped]
        [Required(ErrorMessage = "An image is required.")]
        public IFormFile ImageFile { get; set; }
        [Required]
        public ICollection<int> Genres { get; set; }
        [Required]
        public ICollection<string> GenreTypes { get; set; }
        [Required]
        public ICollection<int> Authors { get; set; }
        [Required]
        public ICollection<string> AuthorsNames { get; set; }
    }
}
