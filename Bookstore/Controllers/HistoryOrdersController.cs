using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Context;
using Bookstore.Models;
using System.Security.Claims;
using Bookstore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    public class HistoryOrdersController : Controller
    {
        private readonly IHistoryService _historyService;

        public HistoryOrdersController(IHistoryService historyService)
        {
            _historyService = historyService;
        }
        [Authorize]
        // GET: HistoryOrders
        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            
            var historyOrders = _historyService.GetHistoryOrders()
                .SelectMany(h => h.Orders)
                .Where(o => o.IdUser == userId)
                .ToList();

            return View(historyOrders);
        }
        [Authorize]
        //GET: HistoryOrders/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = _historyService.GetFinishedOrderById(id.Value);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
