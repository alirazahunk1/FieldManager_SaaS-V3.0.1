using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Order
{
    public class ProductOrderModel : BaseModel
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        public ClientModel? Client { get; set; }

        [MaxLength(50)]
        public string OrderNo { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Total { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Tax { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal GrandTotal { get; set; }

        public int TotalQuantity { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        [MaxLength(500)]
        public string? UserRemarks { get; set; }

        [MaxLength(500)]
        public string? CancelRemarks { get; set; }

        [MaxLength(500)]
        public string? AdminRemarks { get; set; }

        public int? ProcessedById { get; set; }

        public DateTime? ProcessedOn { get; set; }

        public int? CompletedById { get; set; }

        public DateTime? CompletedOn { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public List<OrderLineModel>? OrderLines { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled
    }
}
