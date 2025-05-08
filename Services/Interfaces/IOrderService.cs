using Bookstore.Models;


namespace Bookstore.Services.Interfaces
{
    public interface IOrderService
    {
        Order GetOrCreateCart(int userId);
        void AddToCart(int userId, int bookId, int quantity);
        void RemoveFromCart(int userId, int bookId);
        void ClearCart(int userId);
        //Order FinalizeOrder(int userId);

        Order GetCartWithItems(int userId);
        Order? GetOrderById(int orderId);
        int? MarkOrderAsFinished(Order order);

        string? GetUserEmailById(int userId);
        void DecreaseStockForOrder(Order order);
        List<string> ValidateStock(Order order);
    }
}
