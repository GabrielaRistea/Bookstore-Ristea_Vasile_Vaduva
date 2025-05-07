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
using Bookstore.Services;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    public class AddressesController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            return int.Parse(userIdClaim.Value);
        }

        // GET: Addresses
        [Authorize(Roles = "user")]
        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var address = _addressService.GetAddressByUserId(userId);
            return View(address);
        }

        //// GET: Addresses/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var address = await _context.Addresses
        //        .Include(a => a.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (address == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(address);
        //}

        // GET: Addresses/Create
        [Authorize(Roles = "user")]
        public IActionResult Create(string ReturnUrl)
        {
            if (!string.IsNullOrEmpty(ReturnUrl))
                TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }


        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public IActionResult Create([Bind("Street,City,County,ZipCode,PhoneNumber")] Address address)
        {
            address.IdUser = GetCurrentUserId();
            _addressService.AddAddress(address);

            if (TempData["ReturnUrl"] is string returnUrl)
                return Redirect(returnUrl);

            return RedirectToAction("Index");
        }



        // GET: Addresses/Edit/5
        [Authorize(Roles = "user")]
        public IActionResult Edit(string ReturnUrl)
        {
            if (!string.IsNullOrEmpty(ReturnUrl))
                TempData["ReturnUrl"] = ReturnUrl;

            var userId = GetCurrentUserId();
            var address = _addressService.GetAddressByUserId(userId);
            return View(address);
        }


        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public IActionResult Edit([Bind("Id,Street,City,County,ZipCode,PhoneNumber")] Address address)
        {
            address.IdUser = GetCurrentUserId();
            _addressService.UpdateAddress(address);

            if (TempData["ReturnUrl"] is string returnUrl)
                return Redirect(returnUrl);

            return RedirectToAction("Index");
        }


        // GET: Addresses/Delete/5
        [Authorize(Roles = "user")]
        public IActionResult Delete(string ReturnUrl)
        {
            if (!string.IsNullOrEmpty(ReturnUrl))
                TempData["ReturnUrl"] = ReturnUrl;

            var userId = GetCurrentUserId();
            var address = _addressService.GetAddressByUserId(userId);
            if (address == null) return NotFound();
            return View(address);
        }


        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            var address = _addressService.GetAddressByUserId(userId);
            if (address != null)
                _addressService.DeleteAddress(address.Id);

            if (TempData["ReturnUrl"] is string returnUrl)
                return Redirect(returnUrl);

            return RedirectToAction("Index");
        }


        //private bool AddressExists(int id)
        //{
        //    return _context.Addresses.Any(e => e.Id == id);
        //}
    }
}
