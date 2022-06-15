using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class DepartmentCourse
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course{ get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<OpenedCourse> OpenedCourses { get; set; }


    }
}
