using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace draft.Controllers
{
    public class QueriesController : Controller
    {
        private readonly DbuniversityContext _context;
        private readonly ResultLogger _resultLogger;


        public QueriesController(DbuniversityContext context, ResultLogger resultLogger)
        {
            _context = context;
            _resultLogger = resultLogger;
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

            _resultLogger.LogResults("Query1", courses);

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
            _resultLogger.LogResults("Query2", students);

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
            _resultLogger.LogResults("Query3", teachers);


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
            _resultLogger.LogResults("Query4", teachers);


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
            _resultLogger.LogResults("Query5", students);


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
            _resultLogger.LogResults("Query6", students);

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
            _resultLogger.LogResults("Query7", students);

            return View("Query7", students);
        }




        public IActionResult SearchQ8()
        {
            return View();
        }
        public async Task<IActionResult> Query8(int creditNumber)
        {
            var teachers = await _context.Teachers
                .FromSqlInterpolated($@"
            SELECT Teacher.*
            FROM Teacher
            JOIN CourseAssignment ON Teacher.Id = CourseAssignment.TeacherId
            JOIN Course ON CourseAssignment.CourseId = Course.CourseId
            WHERE Course.CreditsNumber = {creditNumber}
            AND NOT EXISTS (
                SELECT *
                FROM Course AS C
                LEFT JOIN CourseAssignment AS CA ON C.CourseId = CA.CourseId AND CA.TeacherId = Teacher.Id
                WHERE C.CreditsNumber <> {creditNumber}
                AND CA.TeacherId IS NOT NULL
            )")
                .ToListAsync();
            _resultLogger.LogResults("Query8", teachers);

            return View("Query8", teachers);
        }







        public IActionResult SearchQ9()
        {
            return View();
        }


        public async Task<IActionResult> Query9(int creditThreshold)
        {
            var departments = await _context.Departments
                .FromSqlInterpolated($@"
            SELECT *
            FROM Department d
            WHERE NOT EXISTS (
                SELECT 1
                FROM Course c
                WHERE c.DepartmentId = d.DepartmentId
                GROUP BY c.DepartmentId
                HAVING SUM(c.CreditsNumber) <= {creditThreshold}
            )")
                .ToListAsync();

            var departmentResults = new List<DepartmentResult>();
            foreach (var department in departments)
            {
                var totalCredits = await _context.Courses
                    .Where(c => c.DepartmentId == department.DepartmentId)
                    .SumAsync(c => c.CreditsNumber);

                if (totalCredits > creditThreshold)
                {
                    departmentResults.Add(new DepartmentResult
                    {
                        Department = department,
                        TotalCredits = totalCredits
                    });
                }
            }

            _resultLogger.LogResults("Query9", departmentResults);

            return View("Query9", departmentResults);
        }


    }







}
