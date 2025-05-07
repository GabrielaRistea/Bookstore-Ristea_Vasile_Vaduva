using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }
        public void AddToWishlist(int userId, int bookId)
        {
            var wishlist = _wishlistRepository.GetByUserId(userId);
            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    IdUser = userId,
                    WishlistBooks = new List<WishlistBook>()
                };
                _wishlistRepository.Create(wishlist);
                _wishlistRepository.Save();

                // ✅ Refetch pentru a obține Id-ul generat
                wishlist = _wishlistRepository.GetByUserId(userId);
            }


            if (_wishlistRepository.WishlistBookExists(wishlist.Id, bookId))
            {
                return;
            }

            var book = _wishlistRepository.GetBookById(bookId);
            if (book == null) return;

            var wishlistBook = new WishlistBook
            {
                IdBook = bookId,
                IdWishlist = wishlist.Id
            };

            _wishlistRepository.AddWishlistBook(wishlistBook);
            _wishlistRepository.Save();
        }
        public IEnumerable<WishlistBook> GetUserWishlist(int userId)
        {
            return _wishlistRepository.GetWishlistBooksByUserId(userId);
        }

        public void RemoveFromWishlist(int wishlistBookId)
        {
            _wishlistRepository.RemoveWishlistBook(wishlistBookId);
            _wishlistRepository.Save();
        }
        public bool Exists(int userId, int bookId)
        {
            return _wishlistRepository.Exists(userId, bookId);
        }
        public void RemoveFromWishlistByBook(int userId, int bookId)
        {
            _wishlistRepository.RemoveFromWishlistByBook(userId, bookId);
            _wishlistRepository.Save();
        }
    }
}
