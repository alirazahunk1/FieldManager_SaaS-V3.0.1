using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class BreakModel : BaseModel
    {
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [ForeignKey(nameof(Attendance))]
        public int? AttendanceId { get; set; }

        public AttendanceModel? Attendance { get; set; }

        [MaxLength(100)]
        public string? Remarks { get; set; }
    }
}
