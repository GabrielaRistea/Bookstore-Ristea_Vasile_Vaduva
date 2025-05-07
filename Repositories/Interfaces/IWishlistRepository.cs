using Bookstore.Models;

namespace Bookstore.Repositories.Interfaces
{
    public interface IWishlistRepository
    {
        Wishlist GetByUserId(int userId);
        void Create(Wishlist wishlist);
        void Save();
        Book GetBookById(int bookId);
        void AddWishlistBook(WishlistBook wishlistBook);
        bool WishlistBookExists(int wishlistId, int bookId);
        IEnumerable<WishlistBook> GetWishlistBooksByUserId(int userId);
        void RemoveWishlistBook(int wishlistBookId);
        WishlistBook GetWishlistBookById(int id);
        bool Exists(int userId, int bookId);
        void RemoveFromWishlistByBook(int userId, int bookId);
    }
}
