using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Order
{
    public class OrderLineModel : BaseModel
    {
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public ProductOrderModel? Order { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public ProductModel? Product { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Total { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Tax { get; set; }

        public LineStatusEnum Status { get; set; } = LineStatusEnum.Posted;

    }

    public enum LineStatusEnum
    {
        Posted,
        Voided,
        None
    }
}
