using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<GenreBook> GenreBooks { get; set; }
    }
}
