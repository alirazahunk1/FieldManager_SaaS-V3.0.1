using ESSDataAccess.Enum;
using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Product
{
    public class ProductModel : BaseModel
    {
        [MaxLength(100)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [MaxLength(100)]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [MaxLength(1000)]

        public string? Description { get; set; }

        public CommonStatus Status { get; set; } = CommonStatus.Active;

        [ForeignKey(nameof(ProductCategory))]
        public int ProductCategoryId { get; set; }

        [Display(Name = "Category")]
        public ProductCategoryModel? ProductCategory { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal BasePrice { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Discount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Tax { get; set; }

        [Column(TypeName = "decimal(10, 2)")]

        public decimal? Price { get; set; }

        public int? Stocks { get; set; }

        public string Images { get; set; } = "";

        public bool IsTaxable { get; set; } = false;

        public bool IsDiscountable { get; set; } = false;

        public bool IsAvailable { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public bool IsNew { get; set; } = false;

        public List<OrderLineModel>? OrderLines { get; set; }

    }
}
