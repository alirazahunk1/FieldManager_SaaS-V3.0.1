using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class EmployeeSettingsModel : BaseModel
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }


    }

}
