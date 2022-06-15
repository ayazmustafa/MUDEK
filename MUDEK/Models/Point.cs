using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class Point
    {
        [Key]
        public int Id { get; set; }
        public decimal Score { get; set; }

        [ForeignKey("Question")]
        public virtual int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        [ForeignKey("StudentOpenedCourse")]
        public virtual int StudentOpenedCourseId { get; set; }
        public StudentOpenedCourse StudentOpenedCourse { get; set; }
    }
}
