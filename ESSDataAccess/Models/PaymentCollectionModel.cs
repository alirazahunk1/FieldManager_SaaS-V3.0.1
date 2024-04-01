using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class PaymentCollectionModel : BaseModel
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }

        [ForeignKey(nameof(Client))]
        public int? ClientId { get; set; }

        public ClientModel? Client { get; set; }

        public PaymentTypeEnum PaymentType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [MaxLength(1000)]
        public string? ProofUrl { get; set; }
    }

    public enum PaymentTypeEnum
    {
        Cash,
        Cheque,
        BankTransfer,
        Other,
    }
}
