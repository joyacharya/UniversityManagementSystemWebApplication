using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityManagementSystemWebApplication.Models
{
    [Table("Department")]
    public class Department
    {
        public int DepartmentId { set; get; }

        [Required(ErrorMessage = "Department Code can't be empty.")]
        [Remote("CheckDepartmentName","Department",ErrorMessage = "Department Code alredy exits.")]
        [Display(Name = "Department Code")]
        public string Code { set; get; }

        [Required(ErrorMessage = "Department Name can't be empty.")]
        [Remote("CheckDepartmentCode", "Department", ErrorMessage = "Department Code alredy exits.")]
        [Display(Name = "Department Name")]
        public string Name { set; get; }
    }
}