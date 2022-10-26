using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicaAut_BauteViviana.Areas.Identity.Data;
using MusicaAut_BauteViviana.Models;

namespace MusicaAut_BauteViviana.Controllers
{
    public class AdminController : Controller
    {
        private readonly ChinookContext _context;
        public AdminController(ChinookContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="Administrator, Manager")]
        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var customerList = _context.Customers.OrderByDescending(id => id.CustomerId).Take(15).ToList();
            return View(customerList);
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.SupportRep)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewData["SupportRepId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Company,Address,City,State,Country,PostalCode,Phone,Fax,Email,SupportRepId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupportRepId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", customer.SupportRepId);
            return View(customer);
        }

        // GET: Admin/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }
            ViewBag.ArtistId = new SelectList(_context.Customers, "CustomerId", "Name");
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["SupportRepId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", customer.SupportRepId);
            return View(customer);
        }

        // POST: Admin/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Company,Address,City,State,Country,PostalCode,Phone,Fax,Email,SupportRepId")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["SupportRepId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", customer.SupportRepId);
            return View(customer);
        }

        // GET: Admin/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.SupportRep)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ChinookContext.Customers'  is null.");
            }
            //He comentado esto para que no borre por si acaso, para poder hacer pruebas.
            //var customer = await _context.Customers.FindAsync(id);
            //if (customer != null)
            //{
            //    _context.Customers.Remove(customer);
            //}
            
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
