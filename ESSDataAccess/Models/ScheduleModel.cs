using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models
{
    public class ScheduleModel : BaseModel
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsAutoCheckOutEnabled { get; set; } = false;

        public DateTime? AutoCheckOutTime { get; set; }

        public int? CreatedBy { get; set; }

        public ScheduleStatus Status { get; set; }

        public bool Sunday { get; set; }

        public bool Monday { get; set; }

        public bool Tuesday { get; set; }

        public bool Wednesday { get; set; }

        public bool Thursday { get; set; }

        public bool Friday { get; set; }

        public bool Saturday { get; set; }

        public List<AppUser>? Users { get; set; }

    }
}
