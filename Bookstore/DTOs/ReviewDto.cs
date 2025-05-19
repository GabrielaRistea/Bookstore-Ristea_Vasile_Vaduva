using System.ComponentModel.DataAnnotations;

namespace Bookstore.DTOs
{
    public class ReviewDto
    {
        [Required]
        public int IdBook { get; set; }

        // Presupunem că IdUser va fi completat din contextul utilizatorului logat
        public int IdUser { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comm { get; set; }

        [Required]
        [Range(0, 5)]
        public float Rating { get; set; }
    }
}
