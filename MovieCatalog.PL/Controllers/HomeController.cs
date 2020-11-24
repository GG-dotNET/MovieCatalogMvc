using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data.Context;
using MovieCatalog.Model.Models;
using MovieCatalog.PL.Service;
using System.Threading.Tasks;
using System.Linq;

namespace MovieCatalog.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieCatalogContext _context;

        public HomeController(MovieCatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var films = from s in _context.Films
                        select s;

            int pageSize = 4;
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
    }
}
