using Bookstore.Models;
using Bookstore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bookstore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Metodă privată pentru a extrage userId din claims
        public int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            return int.Parse(userIdClaim.Value);
        }

        // GET: /Orders (Coșul de cumpărături)
        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var cart = _orderService.GetCartWithItems(userId);
            return View(cart);
        }

        // POST: /Orders/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int bookId, int quantity)
        {
            var userId = GetCurrentUserId();
            _orderService.AddToCart(userId, bookId, quantity);

            // Setează mesajul de succes în TempData
            TempData["SuccessMessage"] = "Book has been added to your cart!";

            // Redirecționează înapoi la pagina de unde a venit utilizatorul
            return Redirect(Request.Headers["Referer"].ToString());
        }


        // POST: /Orders/RemoveFromCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int bookId)
        {
            var userId = GetCurrentUserId();
            _orderService.RemoveFromCart(userId, bookId);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Orders/ClearCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearCart()
        {
            var userId = GetCurrentUserId();
            _orderService.ClearCart(userId);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Orders/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout()
        {
            var userId = GetCurrentUserId();
            _orderService.FinalizeOrder(userId);
            return RedirectToAction(nameof(OrderConfirmation));
        }

        // GET: /Orders/OrderConfirmation
        public IActionResult OrderConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IncreaseQuantity(int bookId)
        {
            var userId = GetCurrentUserId();
            _orderService.AddToCart(userId, bookId, 1); // Adaugă 1
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DecreaseQuantity(int bookId)
        {
            var userId = GetCurrentUserId();
            var cart = _orderService.GetCartWithItems(userId);
            var item = cart.OrderItems.FirstOrDefault(i => i.IdBook == bookId);
            if (item != null && item.Quantity > 1)
            {
                _orderService.AddToCart(userId, bookId, -1); // Scade 1
            }
            else
            {
                _orderService.RemoveFromCart(userId, bookId); // Dacă ajunge la 0, elimină complet
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
