using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CourseOutcome> DersCiktilari { get; set; }
        public ICollection<DepartmentCourse> DepartmentCourses { get; set; }
    }
}
