using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class Question
    {

        // dersin ismi
        // 1. soru 2. soru 3. soru

        [Key]
        public int Id { get; set; }
        public int WhichQuestion { get; set; }
        public string QuestionText { get; set; }
        public int MaxPoint { get; set; }
        
        [ForeignKey("Exam")]
        public virtual int ExamId{ get; set; }
        public virtual Exam Exam { get; set; }

        [ForeignKey("CourseOutcome")]
        public virtual int CourseOutcomeId { get; set; }
        public virtual CourseOutcome CourseOutcome { get; set; }
        

        public virtual ICollection<Point> Points { get; set; }

    }
}
