using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class CourseOutcome
    {
        [Key]
        public int Id { get; set; }
        public string OutcomeText  { get; set; }

        [ForeignKey("Course")]
        public int CourseId{ get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        

    }
}
