﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mudek.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Faculty { get; set; }
        public ICollection<DepartmentCourse> DepartmentCourses { get; set; }
        public ICollection<DepartmentTeacher> DepartmentTeachers { get; set; }

    }
}
