using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace draft.Controllers
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

        public IActionResult SearchQ2()
        {
            return View();
        }
        public async Task<IActionResult> Query2(string departmentName)
        {
            if (string.IsNullOrEmpty(departmentName))
            {
                return RedirectToAction("Index");
            }

            var students = await _context.Students
                .FromSqlInterpolated($@"
            SELECT DISTINCT Student.*
            FROM Student
            JOIN Studying ON Student.Id = Studying.StudentId
            JOIN Course ON Studying.CourseId = Course.CourseId
            JOIN Department ON Course.DepartmentId = Department.DepartmentId
            WHERE Department.Name = {departmentName}")
                .ToListAsync();

            return View("Query2", students);
        }



        public IActionResult SearchQ3()
        {
            return View();
        }

        public async Task<IActionResult> Query3(int creditThreshold)
        {
            var teachers = await _context.Teachers
                .FromSqlInterpolated($@"
            SELECT DISTINCT Teacher.*
            FROM Teacher
            JOIN CourseAssignment ON Teacher.Id = CourseAssignment.TeacherId
            JOIN Course ON CourseAssignment.CourseId = Course.CourseId
            WHERE Course.CreditsNumber > {creditThreshold}")
                .ToListAsync();

            return View("Query3", teachers);
        }






        public IActionResult SearchQ4()
        {
            return View();
        }
        public async Task<IActionResult> Query4(string departmentName)
        {
            if (string.IsNullOrEmpty(departmentName))
            {
                return RedirectToAction("Index");
            }

            var teachers = await _context.Teachers
        .FromSqlInterpolated($@"
            SELECT Teacher.*
            FROM Teacher
            INNER JOIN Department ON Teacher.DepartmentId = Department.DepartmentId
            WHERE Department.Name = {departmentName}")
        .ToListAsync();

            return View("Query4", teachers);
        }
        public IActionResult SearchQ5()
        {
            return View();
        }
        public async Task<IActionResult> Query5(string subject)
        {
            if (string.IsNullOrEmpty(subject))
            {
                return RedirectToAction("Index");
            }

            var students = await _context.Students
                .FromSqlInterpolated($@"
            SELECT Student.*
            FROM Student
            JOIN Studying ON Student.Id = Studying.StudentId
            JOIN Course ON Studying.CourseId = Course.CourseId
            WHERE Course.Title = {subject} AND Studying.Grade > 90")
                .ToListAsync();

            return View("Query5", students);
        }









        public IActionResult SearchQ6()
        {
            return View();
        }

        public async Task<IActionResult> Query6(int studentId)
        {
            var students = await _context.Students
                .FromSqlInterpolated($@"
            SELECT *
            FROM Student
            WHERE NOT EXISTS (
                SELECT *
                FROM Studying
                WHERE Studying.StudentId = {studentId}
                AND NOT EXISTS (
                    SELECT *
                    FROM Studying AS S
                    WHERE S.StudentId = Student.Id
                    AND S.CourseId = Studying.CourseId
                )
            )")
                .ToListAsync();

            return View("Query6", students);
        }


        public IActionResult SearchQ7()
        {
            return View();
        }
        public async Task<IActionResult> Query7(string departmentName)
        {
            var students = await _context.Students
                .FromSqlInterpolated($@"
            SELECT *
            FROM Student
            WHERE NOT EXISTS (
                SELECT *
                FROM Course
                WHERE Course.DepartmentId = (
                    SELECT DepartmentId
                    FROM Department
                    WHERE Name = {departmentName}
                )
                AND NOT EXISTS (
                    SELECT *
                    FROM Studying
                    WHERE Studying.StudentId = Student.Id
                    AND Studying.CourseId = Course.CourseId
                )
            )")
                .ToListAsync();

            return View("Query7", students);
        }





        public IActionResult SearchQ8()
        {
            return View();
        }
        public async Task<IActionResult> Query8(int grade)
        {
            var students = await _context.Students
        .FromSqlInterpolated($@"
            SELECT DISTINCT Student.*
            FROM Student
            WHERE NOT EXISTS (
                SELECT *
                FROM Studying AS S1
                WHERE S1.StudentId = Student.Id
                AND NOT EXISTS (
                    SELECT *
                    FROM Studying AS S2
                    WHERE S2.StudentId = Student.Id
                    AND S2.CourseId = S1.CourseId
                    AND S2.Grade > {grade}
                )
            )")
        .ToListAsync();

            return View("Query8", students);
        }

    }
}
