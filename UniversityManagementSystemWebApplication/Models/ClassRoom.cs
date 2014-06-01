using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityManagementSystemWebApplication.Models
{
    [Table("ClassRoom")]
    public class ClassRoom
    {
        public int ClassRoomId { set; get; }
        public string RoomNo { set; get; }
    }
}