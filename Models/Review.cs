using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string Comm { get; set; }
        public float Rating { get; set; }
        [ForeignKey(nameof(User))]
        public int IdUser { get; set; }
        public User User { get; set; }
        [ForeignKey(nameof(Book))]
        public int IdBook { get; set; }
        public Book Book { get; set; }
    }
}
