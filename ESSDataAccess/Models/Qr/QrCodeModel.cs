using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Qr
{
    public class QrCodeModel : BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey(nameof(QrCodeGroup))]
        public int QrCodeGroupId { get; set; }

        public QrCodeGroupModel? QrCodeGroup { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(250)]
        public string? QrCode { get; set; }

        public bool IsEnabled { get; set; } = true;
    }
}
