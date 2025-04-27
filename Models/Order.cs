using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string statusOrder {  get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        [ForeignKey(nameof(User))]
        public int IdUser { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(HistoryOrder))]
        public int IdHistoryOrders { get; set; }
        public HistoryOrders HistoryOrder { get; set; }

    }
}
