using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Site;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Contract
{
    public class ContractModel : BaseModel
    {
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        public ClientModel? Client { get; set; }

        [ForeignKey(nameof(Site))]
        public int? SiteId { get; set; }

        public SiteModel? Site { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ContractStatus Status { get; set; } = ContractStatus.Active;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Value { get; set; }

    }

    public enum ContractStatus
    {
        Active,
        Inactive,
        Hold,
        Cancelled,
    }
}
