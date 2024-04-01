using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Document
{
    public class DocumentRequestModel : BaseModel
    {
        [ForeignKey(nameof(RequestedBy))]
        public int RequestedById { get; set; }

        public AppUser? RequestedBy { get; set; }

        [MaxLength(100)]
        public string? Remarks { get; set; }

        public int? ActionTakenById { get; set; }

        public DateTime? ActionTakenOn { get; set; }

        [MaxLength(100)]
        public string? ApproverRemarks { get; set; }

        [MaxLength(3000)]
        public string? DocumentPath { get; set; }

        [ForeignKey(nameof(DocumentType))]
        public int DocumentTypeId { get; set; }

        public DocumentTypeModel? DocumentType { get; set; }

        public DocumentStatus Status { get; set; } = DocumentStatus.Pending;

    }

    public enum DocumentStatus
    {
        Pending,
        Approved,
        Rejected,
        Generated
    }
}
