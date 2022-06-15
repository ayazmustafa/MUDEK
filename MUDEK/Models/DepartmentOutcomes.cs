using System.ComponentModel.DataAnnotations;

namespace Mudek.Models
{
    public class DepartmentOutcomes
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Outcome { get; set; }
    }
}
