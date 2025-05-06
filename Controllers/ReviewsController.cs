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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        //GET: Reviews
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var reviews = _reviewService.GetAllReviews();
            return View(reviews);
        }

        // GET: Reviews/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _reviewService.GetReviewById(id.Value);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create?idBook=5
        [HttpGet]
        [Authorize(Roles = "user")]
        public IActionResult Create(int bookId)
        {
            var dto = new ReviewDto
            {
                IdBook = bookId
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public IActionResult Create(ReviewDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var review = new Review
            {
                Comm = dto.Comm,
                Rating = dto.Rating,
                IdBook = dto.IdBook,
                IdUser = int.Parse(userIdClaim.Value)
            };

            _reviewService.AddReview(review);
            return RedirectToAction("Details", "Books", new { id = dto.IdBook });
        }



        // GET: Reviews/Edit/5
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _reviewService.GetReviewById(id.Value);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comm,Rating,IdUser,IdBook")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }
            _reviewService.UpdateReview(review);
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(review);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ReviewExists(review.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["IdBook"] = new SelectList(_context.Books, "Id", "Id", review.IdBook);
            //ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Id", review.IdUser);
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _reviewService.GetReviewById(id.Value);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _reviewService.DeleteReview(id);
            return RedirectToAction(nameof(Index));
        }

        //private bool ReviewExists(int id)
        //{
        //    return _context.Reviews.Any(e => e.Id == id);
        //}
    }
}
