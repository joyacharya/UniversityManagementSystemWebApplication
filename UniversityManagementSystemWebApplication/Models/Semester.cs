using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityManagementSystemWebApplication.Models
{
    [Table("Semester")]
    public class Semester
    {
        public int SemesterId { set; get; }
        public string Name { set; get; }
    }
}