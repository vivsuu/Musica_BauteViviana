using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicaAut_BauteViviana.Models;
using System.Linq;
using System.Xml.Linq;

namespace MusicaAut_BauteViviana.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly ChinookContext _context;
        public AlbumsController(ChinookContext context) //Con esta funcion 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Albums.OrderByDescending(Album => Album.AlbumId).Include(album => album.Artist).Take(15).ToList());
        }

        public IActionResult Create(int? id)
        {
            ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name"); //Le estamos diciendo al ViewBag que necesitamos tanto el ArtistID como el Name 
            return View();
        }
        [HttpPost]
        public IActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                album.AlbumId = _context.Albums.Max(a => a.AlbumId) + 1;
                _context.Albums.Add(album);
                _context.SaveChanges(); // Persistencia de datos. 
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }
        //GET: Albums/DetailsAlbum
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name");
            var album = await _context.Albums
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Ej:Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }
            ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name");
            var artist = await _context.Albums.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: Ej:Albums/Edit/5
        [HttpPost]
        public IActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(album).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(album);
        }

        // GET: Ej:Albums/Delete/5
        public async Task<IActionResult> Delete(int? id) 
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }
            var album = await _context.Albums
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Albums == null)
            {
                return Problem("Entity set 'ChinookContext.Artists'  is null.");
            }
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.AlbumId == id);
        }

        public IActionResult GetTracks(int? id, string name)
        {
            ViewBag.Id = id;
            ViewBag.Name = name;
            List<Track> cancionesArtista = new List<Track>();
            cancionesArtista = _context.Tracks.Where(tracks => tracks.AlbumId == id).ToList();
            return View(cancionesArtista);
        }



    }
}
