using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bookstore.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public byte[]? AuthorImage { get; set; }
        [Required]
        public string Description { get; set; }
        [DataType(DataType.Upload)]
        [DisplayName("Image")]
        [NotMapped]
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
