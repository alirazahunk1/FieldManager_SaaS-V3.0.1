using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class CheckInOutModel : BaseModel
    {
        [ForeignKey(nameof(Attendance))]
        public int AttendanceId { get; set; }

        public AttendanceModel? Attendance { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }
    }
}
