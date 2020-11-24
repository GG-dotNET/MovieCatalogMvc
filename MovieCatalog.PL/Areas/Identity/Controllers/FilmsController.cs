using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data.Context;
using MovieCatalog.Model.Models;
using MovieCatalog.PL.Service;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCatalog.PL.Areas.Identity.Controllers
{
    [Authorize]
    public class FilmsController : Controller
    {
        private readonly MovieCatalogContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public FilmsController(MovieCatalogContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var films = from s in _context.Films
                           select s;
            
            int pageSize = 5;
            return View(await PaginatedList<Film>.CreateAsync(films.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,RelaseDate,Director,PosterPath")]
            Film film, IFormFile uploadedFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadedFile != null)
                    {
                        film.PosterPath = uploadedFile.FileName;

                        using (var stream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, "img/",
                            uploadedFile.FileName), FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(stream);
                        }
                    }

                    _context.Add(film);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Возникла проблема с добавлением нового фильма. Обратитесь к администратору сайта.");
            }

            return View(film);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Film film, IFormFile loadedFile)
        {
            if (id != film.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (loadedFile != null)
                    {
                        
                        film.PosterPath = loadedFile.FileName;

                        using (var stream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, "img/",
                            loadedFile.FileName), FileMode.OpenOrCreate))
                        {
                            await loadedFile.CopyToAsync(stream);
                        }
                    }

                    _context.Entry(film).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.ID))
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
            return View(film);
        }

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (film == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Удаление не возможно. Попробуйте снова, или обратитесь к администратору сайта.";
            }

            return View(film);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Films.FindAsync(id);

            if (film == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }

        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.ID == id);
        }
    }
}
