using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class IpAddressModel : BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? IpAddress { get; set; }

        [ForeignKey(nameof(IpGroup))]
        public int IpGroupId { get; set; }

        public IpGroupModel? IpGroup { get; set; }

        public bool IsEnabled { get; set; } = true;
    }
}
