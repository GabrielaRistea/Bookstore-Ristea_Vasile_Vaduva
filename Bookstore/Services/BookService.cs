using Bookstore.Services.Interfaces;
using Bookstore.Repositories.Interfaces;
using Bookstore.Models;
using Bookstore.DTOs;
using Bookstore.Repositories;

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
        public async Task UpdateBookAsync(BookDto bookDto)
        {
            var book = _bookRepository.GetById(bookDto.Id);
            mapBook(bookDto, book);

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
        public GroupGenreDto GetBooksByGenre(int genreId)
        {
            var genre = _bookRepository.GetBookByGenre(genreId);
            if (genre == null) return null;
            var books = genre.GenreBooks?
            .Where(ab => ab.Book != null)
            .Select(ab => new GroupBookDto
            {
                Id = ab.Book.Id,
                Title = ab.Book.Title,
                Price = ab.Book.Price,
                BookImage = ab.Book.BookImage,
                Stock = ab.Book.Stock,
            }).ToList() ?? new List<GroupBookDto>();
            return new GroupGenreDto
            {
                Id = genre.Id,
                GenreType = genre.Type,
                Books = books,
                BookTitle = books.Select(b => b.Title).ToList()
            };
        
        }

        private void mapBook(BookDto dto, Book book)
        {
            book.Title = dto.Title;
            book.Price = dto.Price;
            book.ISBN = dto.ISBN;
            book.Description = dto.Description;
            book.PublishingDate = dto.PublishingDate.ToUniversalTime();
            book.PublishingHouse = dto.PublishingHouse;
            book.BookImage = dto.BookImage;
            book.ImageFile = dto.ImageFile;
            book.AuthorBooks = dto.Authors.Select(a => new AuthorBook() { IdAuthor = a }).ToList();
            book.GenreBooks = dto.Genres.Select(g => new GenreBook() { IdGenre = g }).ToList();
            book.Stock = dto.Stock;
        }
    }
}
