using System.Diagnostics;
using Edu_project.Models;
using EdukateMvc.Services;
using Microsoft.AspNetCore.Mvc;


namespace EdukateMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        // GET: /

        public HomeController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }

        // GET: /Home/About
        public IActionResult About()
        {
            ViewData["Title"] = "About";
            return View();
        }

        // GET: /Home/Courses
        public async Task<IActionResult> Courses()
        {
            var courses = await _courseService.GetAllAsync();
            return View(courses);
        }


        // GET: /Home/Detail
        public IActionResult Detail()
        {
            ViewData["Title"] = "Course Detail";
            return View();
        }

        // GET: /Home/Features
        public IActionResult Features()
        {
            ViewData["Title"] = "Our Features";
            return View();
        }

        // GET: /Home/Team
        public IActionResult Team()
        {
            ViewData["Title"] = "Instructors";
            return View();
        }

        // GET: /Home/Testimonial
        public IActionResult Testimonial()
        {
            ViewData["Title"] = "Testimonial";
            return View();
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact";
            return View();
        }


        public IActionResult Privacy() => View();

    }
}

