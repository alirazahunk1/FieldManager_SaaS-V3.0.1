using ESSDataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSDataAccess.Dtos.API_Dtos.Attendance.CheckOut
{
    public class CheckOutDto
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public int? ScheduleId { get; set; }
        public DateTime CheckOut { get; set; }
        public AttendanceStatus Status { get; set; }
        public decimal CheckOutLatitude { get; set; }
        public decimal CheckOutLongitude { get; set; }
    }
}
