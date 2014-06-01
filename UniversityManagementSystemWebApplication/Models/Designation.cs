using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityManagementSystemWebApplication.Models
{
    [Table("Designation")]
    public class Designation
    {
        public int DesignationId { set; get; }
        public string Name { set; get; }
    }
}