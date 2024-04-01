using ESSDataAccess.Enum;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models.Document
{
    public class DocumentTypeModel : BaseModel
    {
        [Required(ErrorMessage = "Leave Type Name is required")]
        [MaxLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Name { get; set; }

        public CommonStatus Status { get; set; } = CommonStatus.Active;

        public List<DocumentRequestModel>? DocumentRequests { get; set; }
    }
}
