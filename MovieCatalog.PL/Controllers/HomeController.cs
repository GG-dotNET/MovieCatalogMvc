using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data.Context;
using MovieCatalog.Model.Models;
using MovieCatalog.PL.Service;
using System.Threading.Tasks;
using System.Linq;
using NLog;

namespace MovieCatalog.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieCatalogContext _context;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public HomeController(MovieCatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            _logger.Debug("Получаем список фильмов из БД");

            var films = from s in _context.Films
                        select s;

            int pageSize = 4;
            return View(await PaginatedList<Film>.CreateAsync(films.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            _logger.Debug("Получаем детальную информацию фильма из БД");

            if (id == null)
            {
                _logger.Warn("Отсутствует или неверный Id фильма");
                return NotFound();
            }

            var film = await _context.Films
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (film == null)
            {
                _logger.Warn("Фильм отсутствует в БД");
                return NotFound();
            }

            _logger.Debug("Информация получена");

            return View(film);
        }
    }
}
