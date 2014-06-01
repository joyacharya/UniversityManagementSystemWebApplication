using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace UniversityManagementSystemWebApplication.Models
{
    [Table("Teacher")]
    public class Teacher
    {
        public int TeacherId { set; get; }

        
        [Display(Name = "Teacher Name")]
        public string Name { set; get; }

        
        [Display(Name = "Teacher Address")]
        public string Address { set; get; }

        [Required]
        [Display(Name = "Teacher Email")]
        [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", ErrorMessage = "Invalid e-mail.")]
        [Remote("CheckTeacherEmail","Teacher",ErrorMessage = "Email address already exists.")]
        public string Email { set; get; }

        [Required]
        [Display(Name = "Teacher Contract Number")]
        public int ContractNo { set; get; }

        [Required]
        [Display(Name = "Designation Name")]
        public int DesignationId { set; get; }

        [Required]
        [Display(Name = "Department Id")]
        public int DepartmentId { set; get; }

        [Required]
        [Display(Name = "Credit To Be Taken")]
        public double CreditToBeTaken { set; get; }

        [Display(Name = "Reamining Credit")]
        public double RemainingCredit { set; get; }

        public virtual Designation Designation { set; get; }
        public virtual Department Department { set; get; }
    }
}