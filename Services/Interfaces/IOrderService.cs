using Bookstore.Models;


namespace Bookstore.Services.Interfaces
{
    public interface IOrderService
    {
        Order GetOrCreateCart(int userId);
        void AddToCart(int userId, int bookId, int quantity);
        void RemoveFromCart(int userId, int bookId);
        void ClearCart(int userId);
        void FinalizeOrder(int userId);
        Order GetCartWithItems(int userId);
    }
}
