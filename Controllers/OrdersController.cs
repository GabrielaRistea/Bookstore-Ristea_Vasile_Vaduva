using Bookstore.Models;
using Bookstore.Services;
using Bookstore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bookstore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMailService _mailService;
        private readonly IAddressService _addressService;

        public OrdersController(IOrderService orderService, IMailService mailService, IAddressService addressService)
        {
            _orderService = orderService;
            _mailService = mailService;
            _addressService = addressService;
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            return int.Parse(userIdClaim.Value);
        }

        // GET: /Orders (Coșul de cumpărături)
        [Authorize]
        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var cart = _orderService.GetCartWithItems(userId);
            return View(cart);
        }



        // POST: /Orders/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
        [Authorize]
        public IActionResult RemoveFromCart(int bookId)
        {
            var userId = GetCurrentUserId();
            _orderService.RemoveFromCart(userId, bookId);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Orders/ClearCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult ClearCart()
        {
            var userId = GetCurrentUserId();
            _orderService.ClearCart(userId);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Orders/Checkout
        [Authorize]
        public IActionResult Checkout()
        {
            var userId = GetCurrentUserId();
            var cart = _orderService.GetCartWithItems(userId);
            if (cart == null)
            {
                return NotFound();
            }

            var address = _addressService.GetAddressByUserId(userId);
            ViewBag.Address = address; // o trimiți către view


            return View(cart);
        }


        // POST: /Orders/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult PlaceOrder(string DeliveryMethod, string PaymentMethod, string TotalPrice)
        {
            var userId = GetCurrentUserId();
            var cart = _orderService.GetCartWithItems(userId);
            if (cart == null)
                return NotFound();

            var address = _addressService.GetAddressByUserId(userId);
            if (address == null)
                return NotFound("Address not found.");

            var stockErrors = _orderService.ValidateStock(cart);
            if (stockErrors.Any())
            {
                TempData["StockErrors"] = string.Join("<br>", stockErrors);
                return RedirectToAction("Checkout");
            }

            var orderId = _orderService.MarkOrderAsFinished(cart);
            _orderService.DecreaseStockForOrder(cart);


            return RedirectToAction("OrderConfirmation", "Orders", new { id = orderId });
        }

        [Authorize]
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return NotFound();
            var defaultDeliveryFee = 9.99f;
            var total = defaultDeliveryFee + order.OrderItems.Sum(item => item.Book.Price * item.Quantity);
            var date = order.Date.ToString("dd.MM.yyyy HH:mm");

            var address = _addressService.GetAddressByUserId(order.IdUser);
            var addressDetails = address != null ? $"{address.Street}, {address.City}, {address.ZipCode}, {address.County}" : "No address available";

            var itemDetails = string.Join("\n", order.OrderItems.Select(item => $"{item.Book.Title} (x{item.Quantity})"));

            var mailBody = $@"
                Thank you for your order!

                Order Number: #{order.Id}
                Date: {date}
                Total: {total:0.00} Lei
                Delivery Address: {addressDetails}

                Items ordered:
                {itemDetails}

                We appreciate your trust in Bookstore!
                ";

            var mail = new MailMessage
            {
                Subject = "Order Confirmation - Bookstore",
                Body = mailBody,
                IsBodyHtml = false
            };

            var userEmail = _orderService.GetUserEmailById(order.IdUser);
            if (!string.IsNullOrEmpty(userEmail))
            {
                mail.To.Add(userEmail);
                mail.From = new MailAddress("MS_1jRdlE@test-eqvygm003x8l0p7w.mlsender.net", "Bookstore App");
                await _mailService.SendEmailAsync(mail);
            }

            _orderService.ClearCart(order.IdUser);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult IncreaseQuantity(int bookId)
        {
            var userId = GetCurrentUserId();
            _orderService.AddToCart(userId, bookId, 1); // Adaugă 1
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
