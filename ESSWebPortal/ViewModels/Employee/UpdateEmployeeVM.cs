using DocumentFormat.OpenXml.Wordprocessing;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSWebPortal.ViewModels.Employee
{
    public class UpdateEmployeeVM
    {
        public int Id { get; set; }

        [Required, Display(Name = "Team")]
        public int TeamId { get; set; }

        [Required, Display(Name = "Schedule")]
        public int ScheduleId { get; set; }

        [Required, Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? Address { get; set; }

        [Required, Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Alternate Number")]
        public string? AlternateNumber { get; set; }

        [Required, DisplayName("Reporting Manager")]
        public int ParentId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required, MinLength(6), Display(Name = "User Name")]
        public string? UserName { get; set; }

        [Display(Name = "Unique Id (Ex. Aadhar, SSN)")]
        public string? UniqueId { get; set; }

        public UserGender Gender { get; set; }

        [Display(Name = "Date Of Joining")]
        public DateTime DateOfJoin { get; set; }

        [Display(Name = "Base Salary Per Month")]
        public decimal BaseSalary { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime Dob { get; set; }

        public string Designation { get; set; }

        [Display(Name = "Available Leave Count")]
        public int AvailableLeaveCount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Primary Sales Target")]
        public decimal? PrimarySalesTarget { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Secondary Sales Target")]
        public decimal? SecondarySalesTarget { get; set; }

        public string? ImgUrl { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? File { get; set; }

        [DisplayName("Geofence Group")]
        public int? GeofenceGroupId { get; set; }

        [DisplayName("IP Group")]
        public int? IpGroupId { get; set; }

        [DisplayName("Qr Group")]
        public int? QrGroupId { get; set; }

        [Display(Name = "Dynamic QR")]
        public int? DynamicQrId { get; set; }

        [DisplayName("Site")]
        public int? SiteId { get; set; }


        [DisplayName("Attendance Type")]
        public EmployeeAttendanceTypeEnum AttendanceType { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
