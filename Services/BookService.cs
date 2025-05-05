using Bookstore.Services.Interfaces;
using Bookstore.Repositories.Interfaces;
using Bookstore.Models;

namespace Bookstore.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public Book GetBookById(int id)
        {
            return _bookRepository.GetById(id);
        }

        public async Task AddBookAsync(Book book)
        {
            if (book.ImageFile != null && book.ImageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await book.ImageFile.CopyToAsync(ms);
                    book.BookImage = ms.ToArray();
                }
            }
            _bookRepository.Create(book);
            _bookRepository.Save();
        }
        public async Task UpdateBookAsync(Book book)
        {
            using var ms = new MemoryStream();

            if (book.ImageFile != null && book.ImageFile.Length > 0)
            {
                await book.ImageFile.CopyToAsync(ms);
                book.BookImage = ms.ToArray();
            }
            _bookRepository.Update(book);
            _bookRepository.Save();
        }
        public void DeleteBook(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book != null)
            {
                _bookRepository.Delete(book);
                _bookRepository.Save();
            }
        }
        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAll().ToList();
        }
        public List<AuthorBook> GetAllAuthorBooks()
        {
            return _bookRepository.GetAllAuthorBooks().ToList();
        }

        public List<GenreBook> GetAllGenreBooks()
        {
            return _bookRepository.GetAllGenreBooks().ToList();
        }
        public List<Review> GetAllReviews()
        {
            return _bookRepository.GetAllReviews().ToList();
        }
        public bool BookExists(int id)
        {
            return _bookRepository.BookExists(id);
        }
        public List<OrderItem> GetAllOrderItems()
        {
            return _bookRepository.GetAllOrderItems().ToList();
        }
        public List<WishlistBook> GetAllWishlistBooks()
        {
            return _bookRepository.GetAllWishlistBooks().ToList();
        }
    }
}
