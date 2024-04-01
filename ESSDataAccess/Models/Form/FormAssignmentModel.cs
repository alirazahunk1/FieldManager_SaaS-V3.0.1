using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Form
{
    public class FormAssignmentModel : BaseModel
    {
        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }

        public AppUser? User { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }

        public TeamModel? Team { get; set; }

        public int FormId { get; set; }

        public FormModel? Form { get; set; }

    }
}
