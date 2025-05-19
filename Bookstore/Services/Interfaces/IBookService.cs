using Bookstore.Models;
using Bookstore.DTOs;

namespace Bookstore.Services.Interfaces
{
    public interface IBookService
    {
        Book GetBookById(int id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(BookDto bookDto);
        void DeleteBook(int id);
        List<Book> GetAllBooks();
        List<AuthorBook> GetAllAuthorBooks();
        List<GenreBook> GetAllGenreBooks();
        List<OrderItem> GetAllOrderItems();
        List<WishlistBook> GetAllWishlistBooks();
        List<Review> GetAllReviews();
        bool BookExists(int id);
        GroupGenreDto GetBooksByGenre(int genreId);
    }
}
