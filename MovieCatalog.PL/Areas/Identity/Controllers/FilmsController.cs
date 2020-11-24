using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data.Context;
using MovieCatalog.Model.Models;
using MovieCatalog.PL.Service;
using NLog;
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
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public FilmsController(MovieCatalogContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            _logger.Debug("Получаем список фильмов из БД");

            var films = from s in _context.Films
                           select s;
            
            int pageSize = 5;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,RelaseDate,Director,PosterPath")]
            Film film, IFormFile uploadedFile)
        {
            _logger.Debug("Добавляем новый фильм в БД");

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
                _logger.Warn("Ошибка при создании фильма");
                ModelState.AddModelError("", "Возникла проблема с добавлением нового фильма. Обратитесь к администратору сайта.");
            }

            _logger.Debug("Фильм добавлен в БД");

            return View(film);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.Warn("Отсутствует или неверный Id фильма");
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                _logger.Warn("Фильм отсутствует в БД");
                return NotFound();
            }

            return View(film);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Film film, IFormFile loadedFile)
        {
            _logger.Debug("Редактируем выбранный фильм из БД");

            if (id != film.ID)
            {
                _logger.Warn("Отсутствует или неверный Id фильма");
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
                        _logger.Warn("Ошибка редактирования фильма");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            _logger.Debug("Редактирование прошло успешно");

            return View(film);
        }

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            _logger.Debug("Собираемся удалить фильм из БД");

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

            if (saveChangesError.GetValueOrDefault())
            {
                _logger.Warn("Ошибка удаления фильма");
                ViewData["ErrorMessage"] =
                    "Удаление не возможно. Попробуйте снова, или обратитесь к администратору сайта.";
            }

            return View(film);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.Debug("Подтверждение удаления фильма из БД");

            var film = await _context.Films.FindAsync(id);

            if (film == null)
            {
                _logger.Warn("Фильм отсутствует в БД");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();

                _logger.Debug("Удаление прошло успешно");

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                _logger.Warn("Ошибка удаления фильма");
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.ID == id);
        }
    }
}
