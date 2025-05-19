using Bookstore.Models;

namespace Bookstore.Services.Interfaces
{
    public interface IHistoryService
    {
        IEnumerable<HistoryOrders> GetHistoryOrders();
        Order? GetFinishedOrderById(int orderId);

    }
}
