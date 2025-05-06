using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bookstore.DTOs
{
    public class GroupBookDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public float Price { get; set; }
        public byte[] BookImage { get; set; }
        [DataType(DataType.Upload)]
        [DisplayName("Image")]
        [NotMapped]
        [Required(ErrorMessage = "An image is required.")]
        public IFormFile ImageFile { get; set; }
    }
}
