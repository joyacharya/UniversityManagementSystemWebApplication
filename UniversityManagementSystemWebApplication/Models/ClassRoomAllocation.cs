using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityManagementSystemWebApplication.Models
{
    [Table("ClasRoomAllocation")]
    public class ClassRoomAllocation
    {
        public int ClassRoomAllocationId { set; get; }

        [Display(Name = "Department")]
        public int DepartmentId { set; get; }

        [Display(Name = "Course")]
        public int CourseId { set; get; }

        [Display(Name = "Classs Room")]
        public int ClassRoomId { set; get; }

        [Display(Name = "Day")]
        public int DayId { set; get; }

        [Required(ErrorMessage = "Time From can't be empty")]
        [Display(Name = "Time From")]
        public string TimeFrom { set; get; }

        [Required(ErrorMessage = "Time To can't be empty")]
        [Display(Name = "Time To")]
        public string TimeTo { set; get; }

        public virtual Day Day { set; get; }
        public virtual Department Department { set; get; }
        public virtual Course Course { set; get; }
        public virtual ClassRoom ClassRoom { set; get; }
    }
}