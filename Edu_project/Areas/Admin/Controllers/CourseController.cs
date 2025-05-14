using System.Linq;
using System.Threading.Tasks;
using EdukateMvc.Models;
using EdukateMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EdukateMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _srv;
        private readonly ILogger<CourseController> _log;

        public CourseController(ICourseService srv, ILogger<CourseController> log)
        {
            _srv = srv;
            _log = log;
        }

        public async Task<IActionResult> Index() =>
            View(await _srv.GetAllAsync());

        public IActionResult Create() =>
            View(new Course());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course m)
        {
            if (!ModelState.IsValid)
            {
                foreach (var kv in ModelState.Where(x => x.Value.Errors.Any()))
                    _log.LogWarning("Validation error on {Field}: {Error}",
                        kv.Key, kv.Value.Errors.First().ErrorMessage);
                return View(m);
            }

            await _srv.CreateAsync(m);
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
            await _srv.EditAsync(m);
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
