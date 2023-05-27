using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivDBWebApplication;
using UnivDBWebApplication.Models;

namespace UnivDBWebApplication.Controllers
{
    public class StudyingsController : Controller
    {
        private readonly DbuniversityContext _context;

        public StudyingsController(DbuniversityContext context)
        {
            _context = context;
        }

        // GET: Studyings
        public async Task<IActionResult> Index()
        {
            var dbuniversityContext = _context.Studyings.Include(s => s.Course).Include(s => s.Student);
            return View(await dbuniversityContext.ToListAsync());
        }

        // GET: Studyings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Studyings == null)
            {
                return NotFound();
            }

            var studying = await _context.Studyings
                .Include(s => s.Course)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudyingId == id);
            if (studying == null)
            {
                return NotFound();
            }

            return View(studying);
        }

        // GET: Studyings/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName");

            return View();
        }

        // POST: Studyings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int courseId, int studentId, [Bind("StudentId,CourseId,Grade")] Studying studying)
        {

            studying.CourseId = courseId;
            studying.StudentId = studentId; 
            if (ModelState.IsValid)
            {
                _context.Add(studying);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", studying.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", studying.StudentId);
            return View(studying);
        }

        // GET: Studyings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Studyings == null)
            {
                return NotFound();
            }

            var studying = await _context.Studyings.FindAsync(id);
            if (studying == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", studying.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", studying.StudentId);
            return View(studying);
        }

        // POST: Studyings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudyingId,StudentId,CourseId,Grade")] Studying studying)
        {
            if (id != studying.StudyingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studying);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudyingExists(studying.StudyingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", studying.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", studying.StudentId);
            return View(studying);
        }

        // GET: Studyings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Studyings == null)
            {
                return NotFound();
            }

            var studying = await _context.Studyings
                .Include(s => s.Course)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudyingId == id);
            if (studying == null)
            {
                return NotFound();
            }

            return View(studying);
        }

        // POST: Studyings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Studyings == null)
            {
                return Problem("Entity set 'DbuniversityContext.Studyings'  is null.");
            }
            var studying = await _context.Studyings.FindAsync(id);
            if (studying != null)
            {
                _context.Studyings.Remove(studying);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudyingExists(int id)
        {
          return (_context.Studyings?.Any(e => e.StudyingId == id)).GetValueOrDefault();
        }
    }
}
