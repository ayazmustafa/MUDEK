using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mudek.Extensions;
using Mudek.Models;
using Mudek.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Controllers
{
    [Authorize]
    public class PointController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PointController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var teacherId = User.Identity.GetId();
            var courseId = HttpContext.Session.GetInt32("CourseId");

            var course = _context.Courses.Where(x => x.Id == courseId).Select(x => x.Name).FirstOrDefault();
            ViewBag.courseName = course;
            var examId = id;

            var students = (from s in _context.Students
                            join soc in _context.StudentOpenedCourses on s.Id equals soc.StudentId
                            //join p in _context.Points on soc.Id equals p.StudentOpenedCourseId
                            where soc.OpenedCourseId == courseId
                            select s).ToList();

            var studentsV2 = _context.Students.Include(x => x.StudentOpenedCourses
                                                       .Where(x => x.OpenedCourseId == courseId)).ToList();

            ViewBag.Students = students;
            ViewBag.StudentsV2 = studentsV2;



            // ViewBag'e at js'de de toplamı check et.
            var examMaxNote = _context.Exams.Where(x => x.Id == id).Select(x => x.MaxNote).ToList();

            var points = (from s in _context.Students
                          join soc in _context.StudentOpenedCourses on s.Id equals soc.StudentId
                          join p in _context.Points on soc.Id equals p.StudentOpenedCourseId
                          join q in _context.Questions on p.QuestionId equals q.Id
                          join e in _context.Exams on q.ExamId equals e.Id
                          where e.Id == examId
                          select p).ToList();

            //var pointsV2 = _context.Students.Include(x => x.StudentOpenedCourses)
            //                        .ThenInclude(x => x.Points)
            //                        .ThenInclude(x => x.Question)
            //                        .ThenInclude(x => x.Exam)
            //                        .ToList();

            var qq = (from s in _context.Students
                          join soc in _context.StudentOpenedCourses on s.Id equals soc.StudentId
                          join p in _context.Points on soc.Id equals p.StudentOpenedCourseId
                          join q in _context.Questions on p.QuestionId equals q.Id
                          join e in _context.Exams on q.ExamId equals e.Id
                          where e.Id == examId
                          select q.MaxPoint).ToList();


            ViewBag.MaxPts = qq;

            return View(points);
        }

        [HttpPost]
        public IActionResult SavePoints(IEnumerable<Point> points)
        {
            foreach (var item in points)
            {
                var h = item.QuestionId;
                var ha = item.StudentOpenedCourseId;
                _context.Points.Update(item);
            }
            _context.SaveChanges();

            return Redirect("/Teacher/Index");
        }
    }
}





//var examId = HttpContext.Session.GetInt32("ExamId");

//var students = (from s in _context.Students
//                join soc in _context.StudentOpenedCourses on s.Id equals soc.StudentId
//                where soc.OpenedCourseId == courseId
//                select soc.Id).ToList();


//foreach (var item in students)
//{
//    for (int i = 0; i < exam.NumberOfQuestions; i++)
//    {
//        _context.Points.Add(new Point
//        {
//            QuestionId = i,
//            Score = 0,
//            StudentOpenedCourseId = item
//        });
//    }
//}
//await _context.SaveChangesAsync();
