using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class Semester
    {
        [Key]
        public int Id { get; set; }
        public string SemesterType { get; set; }

        public virtual ICollection<OpenedCourse> OpenedCourses { get; set; }

    }
}
