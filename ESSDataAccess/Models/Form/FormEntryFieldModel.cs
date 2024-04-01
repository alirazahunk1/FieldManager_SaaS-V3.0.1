using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Form
{
    public class FormEntryFieldModel : BaseModel
    {
        [ForeignKey(nameof(FormEntry))]

        public int FormEntryId { get; set; }

        public FormEntryModel? FormEntry { get; set; }

        [ForeignKey(nameof(FormField))]
        public int FormFieldId { get; set; }

        public FormFieldModel? FormField { get; set; }

        public string? Value { get; set; }
    }
}
