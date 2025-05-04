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
using Bookstore.Services;

namespace Bookstore.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            var author = _authorService.GetAllAuthors();
            return View(author);
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _authorService.GetAuthorById(id.Value);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            var author = _authorService.GetAllAuthors();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageFile")] Author author)
        {
            var authors = _authorService.GetAllAuthors();

            await _authorService.AddAuthorAsync(author);
            return RedirectToAction(nameof(Index));
        }

        // GET: Authors/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _authorService.GetAuthorById(id.Value);

            if (_authorService == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, [Bind("Id,Name,Description,ImageFile")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            try
            {
                await _authorService.UpdateAuthorAsync(author);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_authorService.AuthorExists(id))
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

        // GET: Authors/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _authorService.GetAuthorById(id.Value);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var author = _authorService.GetAuthorById(id);
            if (author != null)
            {
                _authorService.DeleteAuthor(id);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult AllAuthorDetails(int authorId)
        {
            var author = _authorService.GetAllAuthorDetails(authorId);


            return View(author);
        }
    }
}
