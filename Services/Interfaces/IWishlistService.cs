using Bookstore.Models;

namespace Bookstore.Services.Interfaces
{
    public interface IWishlistService
    {
        void AddToWishlist(int userId, int bookId);
        IEnumerable<WishlistBook> GetUserWishlist(int userId);
        void RemoveFromWishlist(int wishlistBookId);
        bool Exists(int userId, int bookId);
        void RemoveFromWishlistByBook(int userId, int bookId);
    }
}
