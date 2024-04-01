using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Employee
{
    public class EmployeeVM
    {
        public int Id { get; set; }

        public int? TeamId { get; set; }

        public string TeamName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public int? ParentId { get; set; }

        public string? Email { get; set; }

        [Display(Name = "Profile Picture")]
        public string? ImgUrl { get; set; }

        public string? UserName { get; set; }

        public UserGender Gender { get; set; }

        public UserStatus Status { get; set; }

        public EmployeeAttendanceTypeEnum? EmployeeRestrictionType { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
