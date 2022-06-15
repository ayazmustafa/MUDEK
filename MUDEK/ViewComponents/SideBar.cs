using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mudek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Mudek.Extensions;

namespace Mudek.ViewComponents
{
    public class SideBar : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SideBar(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetId();
                var teacher = _context.Teachers.Where(x => x.Id == Convert.ToInt32(id)) // id'yi güzel getir
                    .Include(x => x.DepartmentTeachers)
                    .ThenInclude(x => x.OpenedCourses)
                    .ThenInclude(x => x.DepartmentCourse)
                    .ThenInclude(x => x.Course)
                    .FirstOrDefaultAsync();
                return View(await teacher);
            }
            return View();
        }


        //select t.FirstName, c.Name from teachers t
        //INNER JOIN departmentteachers dt on t.Id = dt.TeacherId
        //inner join openedcourses oc on oc.DepartmentTeacherId = dt.Id
        //inner join departmentcourses dc on oc.DepartmentCourseId = dc.Id
        //inner join courses c on c.Id = dc.CourseId;
    }
}
