using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class GenreBook
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Book))]
        public int IdBook { get; set; }
        public Book Book { get; set; }

        [ForeignKey(nameof(Genre))]
        public int IdGenre { get; set; }
        public Genre Genre { get; set; }
    }
}
