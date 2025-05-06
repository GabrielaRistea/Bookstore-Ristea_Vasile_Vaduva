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
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: Genres
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var genres = _genreService.GetAllGenres();
            return View(genres);
        }

        // GET: Genres/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = _genreService.GetGenreById(id.Value);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Type")] Genre genre)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(genre);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            _genreService.AddGenre(genre);
            //return View();
            return RedirectToAction("Index", "Home");
            //return View(genre);
        }

        // GET: Genres/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = _genreService.GetGenreById(id.Value);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }
            _genreService.UpdateGenre(genre);
            //return View();
            return RedirectToAction("Index", "Home");
        }

        // GET: Genres/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = _genreService.GetGenreById(id.Value);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _genreService.DeleteGenre(id);
            //return View();
            return RedirectToAction("Index", "Home");
        }

        //private bool GenreExists(int id)
        //{
        //    return _context.Genres.Any(e => e.Id == id);
        //}
    }
}
