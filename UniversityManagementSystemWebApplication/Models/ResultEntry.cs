using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UniversityManagementSystemWebApplication.Models
{
    [Table("ResultEntry")]
    public class ResultEntry
    {
        public int ResultEntryId { set; get; }
        [Required(ErrorMessage = "Student can't be empty")]
        [Display(Name = "Student Reg No")]
        public int StudentId { set; get; }

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
        public string Email { set; get; }

        [Required(ErrorMessage = "Student Department can't be empty")]
        [Display(Name = "Department")]
        public string DepartmentName { set; get; }

        [Required(ErrorMessage = "Student Course can't be empty")]
        [Display(Name = "Select Course")]
        public int CourseId { set; get; }

        [Required(ErrorMessage = "Student Email can't be empty")]
        [Display(Name = "Select Grade Letter")]
        public int GradeLetterId { set; get; }

        public virtual Student Student { set; get; }
        public virtual Course Course { set; get; }
        public virtual GradeLetter GradeLetter { set; get; }

    }
}