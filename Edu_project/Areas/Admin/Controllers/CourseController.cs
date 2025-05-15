using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EdukateMvc.Models;
using EdukateMvc.Services;
using System.Threading.Tasks;

namespace EdukateMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _srv;
        private readonly ILogger<CourseController> _log;

        public CourseController(ICourseService srv,
                                ILogger<CourseController> log)
        {
            _srv = srv;
            _log = log;
        }

        // =====  Список курсов  =====
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses = await _srv.GetAllAsync();
            return View(courses);
        }

        // =====  Экспорт в Excel  =====
        [HttpGet]
        public async Task<FileResult> ExportExcel()
        {
            var courses = await _srv.GetAllAsync();

            using var wb = new ClosedXML.Excel.XLWorkbook();
            var ws = wb.Worksheets.Add("Courses");
            ws.Cell(1, 1).InsertTable(courses);

            using var ms = new System.IO.MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;

            return File(ms.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "courses.xlsx");
        }

        // =====  Create =====
        public IActionResult Create() => View(new Course());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid) return View(course);

            course.CreatedAt = System.DateTime.UtcNow;
            course.Rating = 0;

            await _srv.CreateAsync(course);
            return RedirectToAction(nameof(Index));
        }

        // =====  Edit =====
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _srv.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Course m)
        {
            if (!ModelState.IsValid) return View(m);
            await _srv.UpdateAsync(m);
            return RedirectToAction(nameof(Index));
        }

        // =====  Delete =====
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _srv.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _srv.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
