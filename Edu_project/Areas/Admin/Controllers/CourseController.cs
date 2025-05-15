using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using EdukateMvc.Models;
using EdukateMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EdukateMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService courseService,
                                ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        // =====  Экспорт в Excel  =====
        [HttpGet]
        public async Task<FileResult> ExportExcel()
        {
            var courses = await _courseService.GetAllAsync();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Courses");

            // быстрый способ залить коллекцию целиком
            ws.Cell(1, 1).InsertTable(courses);

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;

            return File(ms.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "courses.xlsx");
        }




        private readonly ICourseService _srv;
        private readonly ILogger<CourseController> _log;



        public async Task<IActionResult> Index() =>
            View(await _srv.GetAllAsync());

        public IActionResult Create() =>
            View(new Course());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                // Логируем ошибки и возвращаем ту же форму
                foreach (var kv in ModelState.Where(x => x.Value.Errors.Any()))
                    _log.LogWarning("Validation error on {Field}: {Error}",
                        kv.Key, kv.Value.Errors.First().ErrorMessage);

                return View(course);
            }

            // Обязательные поля, отсутствующие в форме
            course.CreatedAt = DateTime.UtcNow;
            course.Rating = 0;

            await _srv.CreateAsync(course);   // сервис сам добавит и сохранит
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _srv.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Course m)
        {
            if (!ModelState.IsValid) return View(m);
            await _srv.UpdateAsync(m);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _srv.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _srv.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
