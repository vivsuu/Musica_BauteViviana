using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicaAut_BauteViviana.Models;

namespace MusicaAut_BauteViviana.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly ChinookContext _context;

        public ArtistsController(ChinookContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Artists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Artists.OrderByDescending(artist => artist.ArtistId).Take(15).ToListAsync());
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }
        [Authorize(Roles = "Administrator, Manager")]
        // GET: Artists/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Administrator, Manager")]
        // POST: Artists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistId,Name")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                artist.ArtistId= _context.Artists.Max(a => a.ArtistId) + 1;
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }
        [Authorize(Roles = "Administrator, Manager")]
        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }
        [Authorize(Roles = "Administrator, Manager")]
        // POST: Artists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Artist artist)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(artist).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }
        [Authorize(Roles = "Administrator, Manager")]
        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists.FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }
        [Authorize(Roles = "Administrator, Manager")]
        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Albums == null)
            {
                return Problem("Entity set 'ChinookContext.Artists'  is null.");
            }
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
          return _context.Artists.Any(e => e.ArtistId == id);
        }

        public IActionResult GetDiscos(int? id, string name)
        {
            ViewBag.Id = id;
            ViewBag.Name = name;
            List<Album> discosArtista = new List<Album>(); //Creamos una lista para rellenarla con la siguiente " consulta "
            //Cual es el objeto que quiero? Busco el conjunto de albums
            discosArtista = _context.Albums.Where(album => album.ArtistId == id).ToList();
            return View(discosArtista);
        }
    }
}
