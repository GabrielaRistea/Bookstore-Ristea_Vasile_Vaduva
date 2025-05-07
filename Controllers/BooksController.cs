using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Services.Interfaces;
using Bookstore.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bookstore.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;
        public BooksController(IBookService bookService, IAuthorService authorService, IGenreService genreService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _genreService = genreService;
        }
        [AllowAnonymous]
        public IActionResult Index(string searchBook, string sortOrder)
        {
            var books = _bookService.GetAllBooks().Select(b => mapBook(b)).ToList();
            var authorbooks = _bookService.GetAllAuthorBooks();
            var genrebooks = _bookService.GetAllGenreBooks();
            var orderitems = _bookService.GetAllOrderItems();
            var wishlistbooks = _bookService.GetAllWishlistBooks();
            var reviews = _bookService.GetAllReviews();
            if (!String.IsNullOrEmpty(searchBook))
            {
                books = books.Where(s => s.Title != null ? s.Title.Contains(searchBook) : true).ToList();
            }
            switch (sortOrder)
            {
                case "price_asc":
                    books = books.OrderBy(b => b.Price).ToList(); 
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price).ToList(); 
                    break;
                default:
                    books = books.OrderBy(b => b.Title).ToList(); 
                    break;
            }
            return View(books);
        }
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetBookById(id.Value);

            if (book == null)
            {
                return NotFound();
            }

            return View(mapBook(book));
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            var authors = _authorService.GetAllAuthors();
            var genres = _genreService.GetAllGenres();
            ViewData["Authors"] = new MultiSelectList(authors, "Id", "Name");
            ViewData["Genres"] = new MultiSelectList(genres, "Id", "Type");
            return View(new BookDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Price,ISBN,Description,PublishingDate,PublishingHouse, Authors, Genres, ImageFile")] BookDto bookDto)
        {
            var book = mapBook(bookDto);

            await _bookService.AddBookAsync(book);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetBookById(id.Value);

            if (_bookService == null)
            {
                return NotFound();
            }
            var authors = _authorService.GetAllAuthors();
            var genres = _genreService.GetAllGenres();
            ViewData["Authors"] = new MultiSelectList(authors, "Id", "Name");
            ViewData["Genres"] = new MultiSelectList(genres, "Id", "Type");
            return View(mapBook(book));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,ISBN,Description,PublishingDate,PublishingHouse, Authors, Genres, ImageFile")] BookDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return NotFound();
            }

            var book = mapBook(bookDto);

            //if (ModelState.IsValid)
            //{
            try
            {
               await _bookService.UpdateBookAsync(book);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bookService.BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetBookById(id.Value);

            if (book == null)
            {
                return NotFound();
            }

            return View(mapBook(book));
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book != null)
            {
                _bookService.DeleteBook(id);
            }
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public IActionResult BooksByGenre(int genreId)
        {
            var genreDto = _bookService.GetBooksByGenre(genreId);
            if (genreDto == null)
                return NotFound();

            return View(genreDto);
        }
        private BookDto mapBook(Book b)
        {
            return new BookDto()
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                ISBN = b.ISBN,
                Description = b.Description,
                PublishingDate = b.PublishingDate.ToUniversalTime(),
                PublishingHouse = b.PublishingHouse,
                BookImage = b.BookImage,
                ImageFile = b.ImageFile,
                Authors = b.AuthorBooks?.Select(ab => ab.Author?.Id ?? 0).ToList() ?? [],
                AuthorsNames = b.AuthorBooks?.Select(ab => ab.Author?.Name ?? "").ToList() ?? [],
                Genres = b.GenreBooks?.Select(bg => bg.Genre?.Id ?? 0).ToList() ?? [],
                GenreTypes = b.GenreBooks?.Select(bg => bg.Genre?.Type ?? "").ToList() ?? [],
            };
        }
        private Book mapBook(BookDto bookDto)
        {
            return new Book()
            {
                Id = bookDto.Id,
                Title = bookDto.Title,
                Price = bookDto.Price,
                ISBN = bookDto.ISBN,
                Description = bookDto.Description,
                PublishingDate = bookDto.PublishingDate.ToUniversalTime(),
                PublishingHouse = bookDto.PublishingHouse,
                AuthorBooks = bookDto.Authors.Select(a => new AuthorBook() { IdAuthor = a}).ToList(),
                GenreBooks = bookDto.Genres.Select(g => new GenreBook() { IdGenre = g }).ToList(),
                BookImage = bookDto.BookImage,
                ImageFile = bookDto.ImageFile
            };
        }
    }
}
