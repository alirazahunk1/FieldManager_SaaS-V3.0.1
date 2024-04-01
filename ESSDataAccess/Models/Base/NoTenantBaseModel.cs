using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models.Base
{
    public class NoTenantBaseModel
    {
        [Key]
        [Display(Name = "Sl.No")]
        public int Id { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated On")]
        public DateTime UpdatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
