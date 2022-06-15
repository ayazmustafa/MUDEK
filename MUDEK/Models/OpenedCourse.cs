using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class OpenedCourse
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("Semester")]
        public int SemesterId { get; set; }
        public virtual Semester Semester { get; set; }


        [ForeignKey("DepartmentCourse")]
        public int DepartmentCourseId { get; set; }
        public virtual DepartmentCourse DepartmentCourse { get; set; }

        [ForeignKey("DepartmentTeacher")]
        public int DepartmentTeacherId { get; set; }
        public virtual DepartmentTeacher DepartmentTeacher { get; set; }


        public virtual ICollection<Exam> Exams{ get; set; }
        public virtual ICollection<StudentOpenedCourse> StudentOpenedCourses { get; set; }

    }
}
