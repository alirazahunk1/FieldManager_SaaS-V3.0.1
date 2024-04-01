using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Form
{
    public class FormEntryModel : BaseModel
    {
        [ForeignKey(nameof(Form))]
        public int FormId { get; set; }

        public FormModel? Form { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }

        [ForeignKey(nameof(Client))]
        public int? ClientId { get; set; }

        public ClientModel? Client { get; set; }

        public List<FormEntryFieldModel>? FormEntryFields { get; set; }

    }
}
