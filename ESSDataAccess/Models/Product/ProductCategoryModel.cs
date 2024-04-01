using ESSDataAccess.Enum;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models.Product
{
    public class ProductCategoryModel : BaseModel
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int? ParentId { get; set; }

        public CommonStatus Status { get; set; } = CommonStatus.Active;

        public List<ProductModel>? Products { get; set; }
    }
}
