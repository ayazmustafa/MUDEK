using Mudek.Models;
using System.Collections.Generic;

namespace Mudek.ViewModels
{
    public class StudentGradeViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Point> Points { get; set; }
    }
}
