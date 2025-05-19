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
        [Authorize]
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
        [Authorize]
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
    }
}
