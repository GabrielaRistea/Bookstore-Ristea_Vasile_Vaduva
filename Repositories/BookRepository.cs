using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Repositories
{
    public class BookRepository : IBookRepository
    {
        private BookstoreContext _context;
        public BookRepository(BookstoreContext bookstoreContext)
        {
            _context = bookstoreContext;
        }
        public void Create(Book book)
        {
            _context.Books.Add(book);
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
        }
        public IEnumerable<Book> GetAll()
        {
            return _context.Books
                .Include(a => a.AuthorBooks)
                .ThenInclude(ab => ab.Author)
                .Include(g => g.GenreBooks)
                .ThenInclude(bg => bg.Genre)
                .Include(o => o.OrderItems)
                .Include(w => w.WishlistBooks)
                .Include(r => r.Reviews);
        }
        public List<AuthorBook> GetAllAuthorBooks()
        {
            return _context.AuthorBooks.ToList();
        }

        public List<GenreBook> GetAllGenreBooks()
        {
            return _context.GenreBooks.ToList();
        }
        public List<Review> GetAllReviews()
        {
            return _context.Reviews.ToList();
        }
        public List<OrderItem> GetAllOrderItems()
        {
            return _context.OrderItems.ToList();
        }
        public List<WishlistBook> GetAllWishlistBooks() 
        {
            return _context.WishlistBooks.ToList();
        }
        public Book GetById(int id)
        {
            return _context.Books
                .Include(a => a.AuthorBooks)
                .ThenInclude(ab => ab.Author)
                .Include(g => g.GenreBooks)
                .ThenInclude(bg => bg.Genre)
                .Include(o => o.OrderItems)
                .Include(w => w.WishlistBooks)
                .Include(r => r.Reviews).FirstOrDefault(m => m.Id == id);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public bool BookExists(int id)
        {
            return _context.Books.Any(o => o.Id == id);
        }
        public void Update(Book book)
        {
            _context.Books.Update(book);
        }
        
    }
}

