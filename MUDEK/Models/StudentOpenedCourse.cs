using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class StudentOpenedCourse
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }


        [ForeignKey("OpenedCourse")]
        public int OpenedCourseId { get; set; }
        public virtual OpenedCourse Course { get; set; }


        public ICollection<Point> Points { get; set; }
    }
}
