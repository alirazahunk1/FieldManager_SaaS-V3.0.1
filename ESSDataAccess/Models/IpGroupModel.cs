using ESSDataAccess.Enum;
using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Logs;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models
{
    public class IpGroupModel : BaseModel
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public CommonStatus Status { get; set; } = CommonStatus.Active;

        public List<IpAddressModel>? IpAddresses { get; set; }

        public List<IPVerificationLogModel>? IPVerificationLogs { get; set; }
    }
}
