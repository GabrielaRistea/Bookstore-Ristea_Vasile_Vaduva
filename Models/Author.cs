using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[]? AuthorImage { get; set; }
        [DataType(DataType.Upload)]
        [DisplayName("Imagine")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public ICollection<AuthorBook> AuthorBooks { get; set; }
    }
}
