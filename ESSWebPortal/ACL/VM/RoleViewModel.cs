using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ACL.VM
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [DisplayName("Role Name")]
        public string Name { get; set; }

        public IEnumerable<MvcControllerInfo> SelectedControllers { get; set; }
    }
}
