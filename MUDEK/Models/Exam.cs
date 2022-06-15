using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int NumberOfQuestions { get; set; }
        public int MaxNote { get; set; }
        public string TypeOfExam { get; set; }
        public int ImpactRate { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        
        public virtual ICollection<Question> Questions { get; set; }

        [ForeignKey("OpenedCourse")]
        public int OpenedCourseId { get; set; }
        public virtual OpenedCourse OpenedCourse { get; set; }

    }
}
