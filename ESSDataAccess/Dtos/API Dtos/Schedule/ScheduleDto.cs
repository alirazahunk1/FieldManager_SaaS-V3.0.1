using ESSDataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSDataAccess.Dtos.API_Dtos.Schedule
{
    public class ScheduleDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ScheduleStatus Status { get; set; }
        public ScheduleWeekOff WeekOff { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
