using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSWebPortal.ViewModels.Device;

namespace ESSWebPortal.ViewModels.Employee
{
    public class EmpVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public decimal BaseSalary { get; set; }

        public string AlternatePhoneNumber { get; set; }

        public string? Dob { get; set; }

        public string? DateOfJoining { get; set; }

        public int? ParentId { get; set; }

        public UserGender Gender { get; set; }

        public UserStatus Status { get; set; }

        public int? TeamId { get; set; }

        public string Address { get; set; }

        public int AvailableLeaveCount { get; set; }

        public string? ImgUrl { get; set; }

        public decimal? PrimarySalesTarget { get; set; }

        public decimal? SecondarySalesTarget { get; set; }

        public string Designation { get; set; }

        public string UniqueId { get; set; }

        public int? ScheduleId { get; set; }

        public string ParentName { get; set; }

        public string Schedule { get; set; }

        public string TeamName { get; set; }

        public DeviceVM? Device { get; set; }

        public EmployeeAttendanceTypeEnum AttendanceType { get; set; }

        public int? GeofenceGroupId { get; set; }

        public string GeofenceGroupName { get; set; }

        public int? IpGroupId { get; set; }


        public string IpGroupName { get; set; }


        public int? QrGroupId { get; set; }

        public string QrGroupName { get; set; }


        public int? DynamicQrId { get; set; }


        public string DynamicQrName { get; set; }


        public int? SiteId { get; set; }


        public string SiteName { get; set; }
    }
}
