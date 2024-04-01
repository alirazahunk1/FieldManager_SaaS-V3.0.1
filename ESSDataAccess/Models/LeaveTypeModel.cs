using ESSDataAccess.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models
{
    public class LeaveTypeModel:BaseModel
    {
        [Required(ErrorMessage = "Leave Type Name is required")]
        [MaxLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Name { get; set; }

        [DisplayName("Is Image Required")]
        public bool IsImgRequired { get; set; }

        public ICollection<LeaveRequestModel>? LeaveRequest { get; set; }
    }
}
