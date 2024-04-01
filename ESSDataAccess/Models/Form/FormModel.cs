using ESSDataAccess.Enum;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models.Form
{
    public class FormModel : BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public CommonStatus Status { get; set; } = CommonStatus.Active;

        [Display(Name = "For For")]
        public ForForEnum ForFor { get; set; } = ForForEnum.All;

        [Display(Name = "Is Client Required")]
        public bool IsClientRequired { get; set; }

        public List<FormFieldModel>? FormFields { get; set; }

        public List<FormEntryModel>? FormEntries { get; set; }
    }

    public enum ForForEnum
    {
        All,
        Task,
        Client
    }
}
