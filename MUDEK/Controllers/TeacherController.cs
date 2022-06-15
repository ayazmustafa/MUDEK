using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mudek.Extensions;
using Mudek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }
        // id null ise, ki null XD hocanın ilk dersini bul. onun idsini ver.
        public IActionResult Index(int? id)
        {
            var teacherId = User.Identity.GetId();


            if (id is null)
            {
                var defaultOpenedCourseId = (from e in _context.Exams
                                             join oc in _context.OpenedCourses on e.OpenedCourseId equals oc.Id
                                             join dt in _context.DepartmentTeachers on oc.DepartmentTeacherId equals dt.Id
                                             join t in _context.Teachers on dt.TeacherId equals t.Id
                                             where t.Id == Convert.ToInt32(teacherId)
                                             select oc.Id).FirstOrDefault();

                //var result = (from t in _context.Teachers
                //              join dt in _context.DepartmentTeachers on t.Id equals dt.TeacherId
                //              from oc in _context.OpenedCourses.Where(x => x.DepartmentTeacherId == dt.Id).Take(1).DefaultIfEmpty()
                //              join dc in _context.DepartmentCourses on oc.DepartmentCourseId equals dc.Id
                //              join c in _context.Courses on dc.CourseId equals c.Id
                //              where t.Id == Convert.ToInt32(id)
                //              select new
                //              {
                //                  id = oc.Id,
                //              });
              
                var defaultExams = (from e in _context.Exams
                              join oc in _context.OpenedCourses on e.OpenedCourseId equals oc.Id
                              join dt in _context.DepartmentTeachers on oc.DepartmentTeacherId equals dt.Id
                              join t in _context.Teachers on dt.TeacherId equals t.Id
                              where t.Id == Convert.ToInt32(teacherId)
                              where oc.Id == defaultOpenedCourseId
                              select e).ToList();

                var defaultCourseName = (from e in _context.Exams
                                  join oc in _context.OpenedCourses on e.OpenedCourseId equals oc.Id
                                  join dc in _context.DepartmentCourses on oc.DepartmentCourseId equals dc.Id
                                  join c in _context.Courses on dc.CourseId equals c.Id
                                  join dt in _context.DepartmentTeachers on oc.DepartmentTeacherId equals dt.Id
                                  join t in _context.Teachers on dt.TeacherId equals t.Id
                                  where t.Id == Convert.ToInt32(teacherId)
                                  select c.Name).FirstOrDefault();

                ViewBag.CourseName = defaultCourseName;
                HttpContext.Session.SetInt32("CourseId", (int)defaultOpenedCourseId);
                return View(defaultExams);
            }

            var courseName = (from e in _context.Exams
                              join oc in _context.OpenedCourses on e.OpenedCourseId equals oc.Id
                              join dc in _context.DepartmentCourses on oc.DepartmentCourseId equals dc.Id
                              join c in _context.Courses on dc.CourseId equals c.Id
                              join dt in _context.DepartmentTeachers on oc.DepartmentTeacherId equals dt.Id
                              join t in _context.Teachers on dt.TeacherId equals t.Id
                              where t.Id == Convert.ToInt32(teacherId)
                              where oc.Id == id
                              select c.Name).FirstOrDefault();

            var exams = (from e in _context.Exams
                            join oc in _context.OpenedCourses on e.OpenedCourseId equals oc.Id
                            join dt in _context.DepartmentTeachers on oc.DepartmentTeacherId equals dt.Id
                            join t in _context.Teachers on dt.TeacherId equals t.Id
                            where t.Id == Convert.ToInt32(teacherId)
                            where oc.Id == id
                            select e).ToList();

            ViewBag.CourseName = courseName;

            HttpContext.Session.SetInt32("CourseId", (int)id);
            return View(exams);
        }






    }
}
