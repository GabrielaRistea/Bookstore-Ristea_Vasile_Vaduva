using Bookstore.Models;
using System.Linq.Expressions;

namespace Bookstore.Repositories.Interfaces
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll();
        Book GetById(int id);
        //Book GetByIdWithRelatedEntities(int id);
        bool BookExists(int id);
        void Create(Book book);
        void Update(Book book);
        void Delete(Book book);
        void Save();
        List<AuthorBook> GetAllAuthorBooks();
        List<GenreBook> GetAllGenreBooks();
        List<OrderItem> GetAllOrderItems();
        List<WishlistBook> GetAllWishlistBooks();
        List<Review> GetAllReviews();
        //List<Book> GetBookByAuthor(int authorId);
        //List<Book> GetBookByGenre(int genreId);
        Genre GetBookByGenre(int genreId);
        IQueryable<Book> FindByCondition(Expression<Func<Book, bool>> expression);

    }
}
