using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivDBWebApplication.Models;

namespace UnivDBWebApplication.Controllers
{
    public class QueriesController : Controller
    {
        private readonly DbuniversityContext _context;

        public QueriesController(DbuniversityContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SearchQ1()
        {
            return View();
        }
        public async Task<IActionResult> Query1(string searchString)
        {
            var dbuniversityContext = _context.Courses.Include(c => c.Department);
            ViewData["SearchFilter"] = searchString;
            if (string.IsNullOrEmpty(searchString))
            {
                return RedirectToAction("Index");
            }

            var courses = await _context.Courses
                .FromSqlInterpolated($"SELECT * FROM Course WHERE EXISTS (SELECT 1 FROM CourseAssignment INNER JOIN Teacher ON CourseAssignment.TeacherId = Teacher.Id WHERE CourseAssignment.CourseId = Course.CourseId AND Teacher.LastName = {searchString})")
                .ToListAsync();

            return View(courses);
        }
    }
}
