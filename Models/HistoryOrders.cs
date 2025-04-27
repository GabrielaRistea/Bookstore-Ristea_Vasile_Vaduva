using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class HistoryOrders
    {
        [Key]
        public int Id { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
