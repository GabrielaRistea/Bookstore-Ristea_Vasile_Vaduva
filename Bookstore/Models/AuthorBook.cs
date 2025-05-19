using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class AuthorBook
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Author))]
        public int IdAuthor { get; set; }
        public Author Author { get; set; }

        [ForeignKey(nameof(Book))]
        public int IdBook { get; set; }
        public Book Book { get; set; }
    }
}
