using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        
        public string Mail { get; set; }

        
        public string Password { get; set; }

        

        public virtual ICollection<DepartmentTeacher> DepartmentTeachers{ get; set; }
    }
}
