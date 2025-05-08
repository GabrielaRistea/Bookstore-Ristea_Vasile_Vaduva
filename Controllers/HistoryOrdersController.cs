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

            // Obține doar comenzile finalizate ale utilizatorului cu itemi validați
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

        //// GET: HistoryOrders/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: HistoryOrders/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id")] HistoryOrders historyOrders)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(historyOrders);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(historyOrders);
        //}

        //// GET: HistoryOrders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var historyOrders = await _context.HistoryOrders.FindAsync(id);
        //    if (historyOrders == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(historyOrders);
        //}

        //// POST: HistoryOrders/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id")] HistoryOrders historyOrders)
        //{
        //    if (id != historyOrders.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(historyOrders);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!HistoryOrdersExists(historyOrders.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(historyOrders);
        //}

        //// GET: HistoryOrders/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var historyOrders = await _context.HistoryOrders
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (historyOrders == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(historyOrders);
        //}

        //// POST: HistoryOrders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var historyOrders = await _context.HistoryOrders.FindAsync(id);
        //    if (historyOrders != null)
        //    {
        //        _context.HistoryOrders.Remove(historyOrders);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool HistoryOrdersExists(int id)
        //{
        //    return _context.HistoryOrders.Any(e => e.Id == id);
        //}
    }
}
