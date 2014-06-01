using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace UniversityManagementSystemWebApplication.Models
{
    [Table("Course")]
    public class Course
    {
        public int CourseId { set; get; }
        [Required(ErrorMessage = "Course Code can't be empty.")]
        [Remote("CheckCourseCode","Course",ErrorMessage = "Course Code already exits.")]
        [Display(Name = "Course Code")]
        public string Code { set; get; }
        [Required(ErrorMessage = "Course Name can't be empty.")]
        [Remote("CheckCourseName", "Course", ErrorMessage = "Course Name already exits.")]
        [Display(Name = "Course Name")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Course CreditToBeTaken can't be empty.")]
        [Display(Name = "Course Credit")]
        public double Credit { set; get; }
        public string Description { set; get; }
        [Required]
        [Display(Name = "Deparment Id")]
        public int DepartmentId { set; get; }
        [Required]
        [Display(Name = "Semester Id")]
        public int SemesterId { set; get; }
        public virtual Department Department { set; get; }
        public virtual Semester Semester { set; get; }

        //public int TeacherId { set; get; }
        public virtual Teacher Teacher { set; get; }
    }
}