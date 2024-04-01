using ESSDataAccess.Tenant;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models.Base
{
    public class BaseModel
    {
        [Key]
        [Display(Name = "Sl.No")]
        public int Id { get; set; }
        [Display(Name = "Created On")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Updated On")]
        public DateTime UpdatedAt { get; set; }

        public string Tenant { get; set; } = Tenants.Internet;

        public int TenantId { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
