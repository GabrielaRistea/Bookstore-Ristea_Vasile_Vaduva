using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        [ForeignKey(nameof(Book))]
        public int IdBook { get; set; }
        public Book Book { get; set; }
        [ForeignKey(nameof(Order))]
        public int? IdOrder { get; set; }
        public Order Order { get; set; }
    }
}
