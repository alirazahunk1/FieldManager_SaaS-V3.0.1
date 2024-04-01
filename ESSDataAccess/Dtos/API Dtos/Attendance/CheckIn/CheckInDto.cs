using ESSDataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSDataAccess.Dtos.API_Dtos.Attendance.CheckIn
{
    public class CheckInDto
    {
        public int? AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public int? ScheduleId { get; set; }
        public DateTime CheckIn { get; set; }
        public AttendanceStatus Status { get; set; }
        public decimal CheckInLatitude { get; set; }
        public decimal CheckInLongitude { get; set; }

    }
}
