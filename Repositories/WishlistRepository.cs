using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private BookstoreContext _context;
        public WishlistRepository(BookstoreContext bookstoreContext)
        {
            _context = bookstoreContext;
        }
        public Wishlist GetByUserId(int userId)
        {
            return _context.Wishlists
                .Include(w => w.WishlistBooks)
                .ThenInclude(wb => wb.Book)
                .FirstOrDefault(w => w.IdUser == userId);
        }
        public void Create(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books.Find(bookId);
        }
        public void AddWishlistBook(WishlistBook wishlistBook)
        {
            _context.WishlistBooks.Add(wishlistBook);
        }

        public bool WishlistBookExists(int wishlistId, int bookId)
        {
            return _context.WishlistBooks.Any(wb => wb.IdWishlist == wishlistId && wb.IdBook == bookId);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public IEnumerable<WishlistBook> GetWishlistBooksByUserId(int userId)
        {
            return _context.WishlistBooks
                .Include(wb => wb.Book)
                .Include(wb => wb.Wishlist)
                .Where(wb => wb.Wishlist.IdUser == userId)
                .ToList();
        }
        public void RemoveWishlistBook(int wishlistBookId)
        {
            var item = _context.WishlistBooks.Find(wishlistBookId);
            if (item != null)
            {
                _context.WishlistBooks.Remove(item);
            }
        }
        public WishlistBook GetWishlistBookById(int id)
        {
            return _context.WishlistBooks
                .Include(wb => wb.Book)
                .FirstOrDefault(wb => wb.Id == id);
        }
        public bool Exists(int userId, int bookId)
        {
            return _context.WishlistBooks.Any(w => w.Wishlist.IdUser == userId && w.IdBook == bookId);
        }
        public void RemoveFromWishlistByBook(int userId, int bookId)
        {
            var wishlist = _context.Wishlists
            .FirstOrDefault(w => w.IdUser == userId);

            if (wishlist == null)
            {
                return;
            }
            var wishlistBook = _context.WishlistBooks
        .FirstOrDefault(wb => wb.IdWishlist == wishlist.Id && wb.IdBook == bookId);

            if (wishlistBook == null)
            {
                return;
            }

            _context.WishlistBooks.Remove(wishlistBook);
            _context.SaveChanges();
        }
    }
}
