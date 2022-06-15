using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mudek.Extensions;
using Mudek.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }   


        public IActionResult Create()
        {
            var id = User.Identity.GetId();

            var result = (from t in _context.Teachers
                          join dt in _context.DepartmentTeachers on t.Id equals dt.TeacherId
                          join oc in _context.OpenedCourses on dt.Id equals oc.DepartmentTeacherId
                          join dc in _context.DepartmentCourses on oc.DepartmentCourseId equals dc.Id
                          join c in _context.Courses on dc.CourseId equals c.Id
                          where t.Id == Convert.ToInt32(id)
                          select new
                          {
                              newId = oc.Id,
                              newName = c.Name
                          });
            ViewData["OpenedCourseName"] = new SelectList(result, "newId", "newName");

            return View();
           
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NumberOfQuestions,MaxNote,TypeOfExam,ImpactRate,Description,Date,OpenedCourseId")] Exam exam)
        {
            var courseId = exam.OpenedCourseId;
            if (ModelState.IsValid)
            {
                _context.Add(exam);
                await _context.SaveChangesAsync();

                return Redirect($"/Teacher/Index/{courseId}");
            }
            ViewData["OpenedCourseName"] = new SelectList(_context.OpenedCourses, "newId", "newId", exam.OpenedCourseId);
            return View(exam);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            var teacherId = User.Identity.GetId();

            var result = (from t in _context.Teachers
                          join dt in _context.DepartmentTeachers on t.Id equals dt.TeacherId
                          join oc in _context.OpenedCourses on dt.Id equals oc.DepartmentTeacherId
                          join dc in _context.DepartmentCourses on oc.DepartmentCourseId equals dc.Id
                          join c in _context.Courses on dc.CourseId equals c.Id
                          where t.Id == Convert.ToInt32(teacherId)
                          select new
                          {
                              newId = oc.Id,
                              newName = c.Name
                          });
            ViewData["OpenedCourseName"] = new SelectList(result, "newId", "newName");
            return View(exam);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberOfQuestions,MaxNote,TypeOfExam,ImpactRate,Description,Date,OpenedCourseId")] Exam exam)
        {
            if (id != exam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var courseId = (from e in _context.Exams
                              join oc in _context.OpenedCourses on e.OpenedCourseId equals oc.Id
                              where e.Id == exam.Id
                              select oc.Id).FirstOrDefault();
                return  Redirect($"/Teacher/Index/{courseId}");
            }
            ViewData["OpenedCourseName"] = new SelectList(_context.OpenedCourses, "newId", "newName", exam.OpenedCourseId);
            return View(exam);
        }


        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }

    }
}
