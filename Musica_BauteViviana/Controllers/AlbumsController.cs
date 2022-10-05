using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Musica_BauteViviana.Models;
using System.Linq;

namespace Musica_BauteViviana.Controllers
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
            //List<Album> albumsList = _context.Albums.Take(15).ToList();
            //return View(albumsList);
            return View(_context.Albums.OrderByDescending(Album => Album.AlbumId).Include(album=>album.Artist).Take(15).ToList());
        }

        public IActionResult CreateAlbum()
        {
            ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name"); //Le estamos diciendo al ViewBag que necesitamos tanto el ArtistID como el Name
            //Lo que haces es reemplazarlo en el formulario
            return View();
        }
        [HttpPost]
        public IActionResult CreateAlbum(Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Albums.Add(album);
                _context.SaveChanges(); // Persistencia de datos. 
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
         
        }
    }
}
