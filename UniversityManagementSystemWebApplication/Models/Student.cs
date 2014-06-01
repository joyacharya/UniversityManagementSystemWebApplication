using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityManagementSystemWebApplication.Models
{
    [Table("Student")]
    public class Student
    {
        public int StudentId { set; get; }

        [Display(Name = "Student Registration Number")]
        public string RegNo { set; get; }

        [Required(ErrorMessage = "Student Name can't be empty")]
        [Display(Name = "Student Name")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Student Email can't be empty")]
        [Display(Name = "Student Email")]
        [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+" +
                           "(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+" +
                           ")*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+" +
                           "[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$",
                           ErrorMessage = "Invalid e-mail.")]
        [Remote("CheckMail","Student",ErrorMessage = "This email is already taken")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Student Contact number can't be empty")]
        [Display(Name = "Contact Number")]
        public int ContactNo { set; get; }

        [Required]
        [Display(Name = "Date ")]
        public DateTime Date { set; get; }

        public int DepartmentId { set; get; }
        public virtual Department Department { set; get; }
    }
}