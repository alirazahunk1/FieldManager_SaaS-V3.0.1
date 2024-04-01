using ESSDataAccess.Enum;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models.Qr
{
    public class QrCodeGroupModel : BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public CommonStatus Status { get; set; } = CommonStatus.Active;

        public List<QrCodeModel>? QrCodes { get; set; }
    }
}
