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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Bookstore.DTOs;
using MapsterMapper;
using System.Net;

namespace Bookstore.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly IWishlistService _wishlistService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public WishlistsController(IWishlistService wishlistService, IBookService bookService, IMapper mapper)
        {
            _wishlistService = wishlistService;
            _bookService = bookService;
            _mapper = mapper;
        }
        

        [HttpPost]
        [Authorize]
        public IActionResult AddToWishlist(int bookId, string returnUrl)
        {
            int userId = GetCurrentUserId();

            if (!_wishlistService.Exists(userId, bookId))
            {
                _wishlistService.AddToWishlist(userId, bookId);
                TempData["SuccessMessageW"] = "Added to Wishlist Successfully";
            }
            else
            {
                TempData["SuccessMessageW"] = "Already added to Wishlist";
            }
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Books");
        }
        [HttpPost]
        [Authorize]
        public IActionResult RemoveFromWishlist(int bookId)
        {
            int userId = GetCurrentUserId();
            _wishlistService.RemoveFromWishlistByBook(userId, bookId);

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Index()
        {
            int userId = GetCurrentUserId();
            var wishlistBooks = _wishlistService.GetUserWishlist(userId);

            return View(wishlistBooks);
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
